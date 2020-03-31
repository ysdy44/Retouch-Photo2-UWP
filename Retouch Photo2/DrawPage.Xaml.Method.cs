using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //ViewModel
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
        //KeyboardViewModel
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

        
        //Setup
        private void ConstructSetupDialog()
        {
            this.SetupDialog.CloseButton.Click += (sender, args) => this.SetupDialog.Hide();

            this.SetupDialog.PrimaryButton.Click += (_, __) =>
            {
                this.SetupDialog.Hide();

                BitmapSize size = this.SetupSizePicker.Size;

                this.ViewModel.CanvasTransformer.Width = (int)size.Width;
                this.ViewModel.CanvasTransformer.Height = (int)size.Height;

                this.ViewModel.Invalidate();//Invalidate
            };
        }

        //Save
        private async Task Save()
        {
            string name = this.ViewModel.Name;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;

            await FileUtil.SaveImageRes();
            await FileUtil.SaveProject(this.ViewModel.Layers.RootLayers, name, width, height);
            await FileUtil.CreateFromDirectory(name);

            Func<Matrix3x2, ICanvasImage> renderAction = this.MainCanvasControl.Render;
            FileUtil.SaveThumbnailAsync(this.ViewModel.CanvasDevice, renderAction, name, width, height);
        }


        //Navigated
        private void NavigatedTo()
        {
            //Transition
            Vector2 offset = this.DrawLayout.FullScreenOffset;
            float width = this.DrawLayout.CenterChildWidth;
            float height = this.DrawLayout.CenterChildHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(offset, width, height);

            if (this.ViewModel.IsTransition)
            {
                this.Transition = 0;
                this.TransitionStoryboard.Begin();//Storyboard
               
            }
            else this.NavigatedToComplete();
        }
        private void NavigatedToComplete()
        {
            //Transition
            this.ViewModel.CanvasTransformer.Transition(1.0f);

            this.IsFullScreen = false;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }
        private async Task NavigatedFrom()
        {
            //FileUtil
            await FileUtil.DeleteCacheAsync();

            //Clear
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