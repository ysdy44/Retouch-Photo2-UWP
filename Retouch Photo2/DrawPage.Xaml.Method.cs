using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        private void ConstructViewModel()
        {
            this.MainCanvasControl.ConstructViewModel();

            this.ViewModel.TipWidthHeight += (transformer, point, invalidateMode) =>
            {
                //Text
                {
                    Vector2 horizontal = transformer.Horizontal;
                    Vector2 vertical = transformer.Vertical;

                    int width = (int)horizontal.Length();
                    int height = (int)vertical.Length();

                    this.TipWRun.Text = width.ToString();
                    this.TipHRun.Text = height.ToString();
                }

                //Width Height
                {
                    Vector2 offset = this.DrawLayout.FullScreenOffset;
                    double x = offset.X + point.X + 10;
                    double y = offset.Y + point.Y - 50;

                    Canvas.SetLeft(this.TipToolTip, x);
                    Canvas.SetTop(this.TipToolTip, y);
                }

                switch (invalidateMode)
                {
                    case InvalidateMode.None: break;
                    case InvalidateMode.Thumbnail: this.TipToolTip.Visibility = Visibility.Collapsed; break;
                    case InvalidateMode.HD: this.TipToolTip.Visibility = Visibility.Visible; break;
                }
            };
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
            this.ViewModel.CanvasTransformer.TransitionDestination(offset, width, height);

            this.Transition = 0;
            this.TransitionStoryboard.Begin();//Storyboard
        }
        private void NavigatedToComplete()
        {
            this.ViewModel.CanvasTransformer.Transition(1.0f);
            this.IsFullScreen = false;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        private async void NavigatedFrom()
        {
            this.SelectionViewModel.SetModeNone();
            this.ViewModel.Layers.RootLayers.Clear();
            this.ViewModel.Layers.RootControls.Clear();

            this.IsFullScreen = true;
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

            await Task.Delay(400);
            this.Frame.GoBack();
        }

    }
}