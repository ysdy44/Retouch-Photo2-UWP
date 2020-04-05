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

        //Save
        private async Task Save()
        {
            string name = this.ViewModel.Name;
            int width = this.ViewModel.CanvasTransformer.Width;
            int height = this.ViewModel.CanvasTransformer.Height;

            //Save project to zip file.
            await FileUtil.SaveImageRes();
            await FileUtil.SaveProject(this.ViewModel.Layers.RootLayers, name, width, height);
            await FileUtil.CreateZipFile(name);

            //Save thumbnail image.
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
            await FileUtil.DeleteAllInTemporaryFolder();

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