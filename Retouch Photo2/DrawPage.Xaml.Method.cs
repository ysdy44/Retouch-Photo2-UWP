using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {


        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();
        }

        private void ConstructKeyboardViewModel()
        {
            //Move
            if (this.KeyboardViewModel.Move == null)
            {
                this.KeyboardViewModel.Move += (value) =>
                {
                    this.ViewModel.CanvasTransformer.Position += value;
                    this.ViewModel.CanvasTransformer.ReloadMatrix();
                    this.ViewModel.Invalidate();//Invalidate
                };
            }

            //FullScreen
            if (this.KeyboardViewModel.FullScreenChanged == null)
            {
                this.KeyboardViewModel.FullScreenChanged += (isFullScreen) =>
                {
                    this.IsFullScreen = isFullScreen;
                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }
        

        private void XElementSave(string path)
        {
            //1.Create an XDocument object.
            XDocument xDoc = new XDocument
            {
                //Set the document definition for xml.
                Declaration = new XDeclaration("1.0", "utf-8", "no"),
            };

            //2.Create a root
            XElement root = new XElement("Root");
            xDoc.Add(root);

            //3.WIdth and height.
            root.Add(new XElement("Width", this.ViewModel.CanvasTransformer.Width));
            root.Add(new XElement("Height", this.ViewModel.CanvasTransformer.Height));

            //4.Layers
            XElement layers = new XElement("Layers");
            root.Add(layers);
            {
                foreach (ILayer layer in this.ViewModel.Layers.RootLayers)
                {
                    XElement element = layer.Save();
                    layers.Add(element);
                }
            }

            //5.Save
            xDoc.Save(path);
        }
                 

        private void NavigatedTo()
        {
            //Theme
            ElementTheme theme = this.SettingViewModel.ElementTheme;
            this.RequestedTheme = theme;
            this.ThemeControl.Theme = theme;
            ApplicationViewTitleBarBackgroundExtension.SetTheme(theme);

            //Layout
            this.DrawLayout.VisualStateDeviceType = this.SettingViewModel.LayoutDeviceType;
            this.DrawLayout.VisualStatePhoneMaxWidth = this.SettingViewModel.LayoutPhoneMaxWidth;
            this.DrawLayout.VisualStatePadMaxWidth = this.SettingViewModel.LayoutPadMaxWidth;

            //Transition
            Vector2 offset = this.DrawLayout.FullScreenOffset;
            float width = this.DrawLayout.CenterChildWidth;
            float height = this.DrawLayout.CenterChildHeight;
            this.ViewModel.TransitionDestination(offset, width, height);

            this.Transition = 0;
            this.TransitionStoryboard.Begin();//Storyboard
        }
        private void NavigatedToComplete()
        {
            this.ViewModel.Transition(1.0f);
            this.IsFullScreen = false;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        private async void NavigatedFrom()
        {
            this.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            await Task.Delay(400);
            this.Frame.GoBack();
        }

    }
}