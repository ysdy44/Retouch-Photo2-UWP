using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        /// <summary> The transition data. </summary>
        Rect? _lockSourceRect;


        private void RegisteTransition()
        {
            this.LoadingControl.Completed += (s, e) => this.TransitionComplete();

            this.LoadingControl.ValueChanged += (s, e) =>
            {
                float value = (float)e.NewValue;
                this.TransitionDelta(value);
            };
        }


        //Staring
        private void TransitionStaring()
        {
            this.LoadingControl.State = LoadingState.LoadingWithProgress;

            //Destination
            Vector2 destinationPostion = this.SettingViewModel.FullScreenOffset;
            float destinationWidth = this.SettingViewModel.CanvasWidth;
            float destinationHeight = this.SettingViewModel.CanvasHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(destinationPostion, destinationWidth, destinationHeight);


            if (this._lockSourceRect is Rect data)
            {
                //Source
                this.ViewModel.CanvasTransformer.TransitionSource(data);
                this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate

                this.LoadingControl.Begin();//Storyboard
            }
            else this.TransitionComplete();
        }

        //Delta
        private void TransitionDelta(float value)
        {
            this.ViewModel.CanvasTransformer.Transition(value);
            this.ViewModel.Invalidate(InvalidateMode.None);//Invalidate
        }

        //Complete
        private void TransitionComplete()
        {
            this.LoadingControl.State = LoadingState.None;

            //Transition
            this.ViewModel.CanvasTransformer.Transition(1.0f);

            this.DrawLayout.IsFullScreen = false;

            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


    }
}