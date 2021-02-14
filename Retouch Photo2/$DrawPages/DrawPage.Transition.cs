using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {
        /// <summary>
        /// Loaded.
        /// </summary>
        private void _lockLoaded()
        {
            if (DrawPage._lockIsLoaded == false)
            {
                DrawPage._lockIsLoaded = true;

                this.Transition();
            }
        }
        /// <summary>
        /// OnNavigatedTo.
        /// </summary>
        /// <param name="data"> The data. </param>
        private void _lockOnNavigatedTo(Rect? data)
        {           
            this._lockSourceRect = data;

            if (DrawPage._lockIsLoaded == true)
            {
                this.Transition();
            }
        }

        //@Static
        /// <summary>     
        /// Is this Loaded? 
        /// 
        /// The first time the page is loaded, 
        /// <see cref="Page.OnNavigatedTo"/> is executed before <see cref="Page.Loaded"/>
        /// <see cref="Page.Loaded"/> Responsible for <see cref="Page.NavigatedTo"/>
        /// </summary>
        static bool _lockIsLoaded = false;
        /// <summary> The transition data. </summary>
        Rect? _lockSourceRect;
        


        //Transition
        private void RegisteTransition()
        {
            this.TransitionKeyFrames.Completed += (s, e) => this.TransitionComplete();
            
            this.TransitionSlider.ValueChanged += (s, e) =>
            {
                float value = (float)e.NewValue;
                this.TransitionDelta(value);
            };
        }

        private void Transition()
        {
            //Destination
            Vector2 destinationPostion = this.SettingViewModel.FullScreenOffset;
            float destinationWidth = this.SettingViewModel.CenterChildWidth;
            float destinationHeight = this.SettingViewModel.CenterChildHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(destinationPostion, destinationWidth, destinationHeight);

            if (this._lockSourceRect is Rect data)
            {
                //Source
                this.ViewModel.CanvasTransformer.TransitionSource(data);
                this.TransitionStaring();

                this.TransitionSlider.Value = 0.0d;
                this.TransitionStoryboard.Begin();//Storyboard}
            }
            else this.TransitionComplete();
        }



        //Staring
        private void TransitionStaring()
        {
          //  this.ViewModel.CanvasTransformer.Radian(0.0f);
          //  this.ViewModel.CanvasTransformer.Transition(0.0f);
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
            this.TransitionBorder.Visibility = Windows.UI.Xaml.Visibility.Visible;
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
            //Transition
            this.ViewModel.CanvasTransformer.Transition(1.0f);

            Retouch_Photo2.DrawPage.FullScreen?.Invoke();
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
            this.TransitionBorder.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }


    }
}