using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2
{
    /// <summary>
    /// Type of Transition.
    /// </summary>
    public enum TransitionType
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Page navigate page with Size. </summary>
        Size,
        /// <summary> Page navigate with Transition. </summary>
        Transition,
    }

    /// <summary>
    /// Data of Transition.
    /// </summary>
    public struct TransitionData
    {
        /// <summary> Type of Transition.. </summary>
        public TransitionType Type;
        /// <summary> Transition source rect. </summary>
        public Rect SourceRect;
        /// <summary> Page actually size. </summary>
        public Size PageSize;
    }

    /// <summary> 
    /// Represents a page used to draw vector graphics.
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        // The first time the page is loaded, 
        // ""OnNavigatedTo"" is executed before ""Loaded""
        // ""Loaded"" Responsible for ""NavigatedTo""

        //@Static
        /// <summary> Is this Loaded? </summary>
        static bool _lockIsLoaded = false;
        /// <summary> Disposable data. </summary>
        TransitionData _lockData;

        #region Lock

                  
        /// <summary>
        /// Loaded.
        /// </summary>
        private void _lockLoaded()
        {
            if (DrawPage._lockIsLoaded == false)
            {
                DrawPage._lockIsLoaded = true;

                this.Transition(this._lockData);
            }
        }

        /// <summary>
        /// OnNavigatedTo.
        /// </summary>
        /// <param name="data"> The data. </param>
        private void _lockOnNavigatedTo(TransitionData data)
        {
            if (DrawPage._lockIsLoaded == true)
            {
                this.Transition(data);
            }
            else
            {
                this._lockData = data;
            }
        }


        #endregion


        #region DependencyProperty


        /// <summary> Gets or sets the canvas transition value. </summary>
        public float TransitionValue
        {
            get => (float)base.GetValue(TransitionValueProperty);
            set => base.SetValue(TransitionValueProperty, value);
        }
        /// <summary> Identifies the <see cref = "DrawPage.TransitionValue" /> dependency property. </summary>
        public static readonly DependencyProperty TransitionValueProperty = DependencyProperty.Register(nameof(TransitionValue), typeof(float), typeof(DrawPage), new PropertyMetadata(0.0f, (sender, e) =>
        {
            DrawPage control = (DrawPage)sender;

            if (e.NewValue is float value)
            {
                control.ViewModel.CanvasTransformer.Transition(value);
                control.ViewModel.Invalidate();//Invalidate
            }
        }));


        #endregion
        
        //Transition
        private void ConstructTransition()
        {
            //Binding own DependencyProperty to the Storyboard
            Storyboard.SetTarget(this.TransitionKeyFrames, this);
            Storyboard.SetTargetProperty(this.TransitionKeyFrames, nameof(this.TransitionValue));
            this.TransitionKeyFrames.Completed += (s, e) => this._transitionComplete();
        }

        //Transition
        private void Transition(TransitionData data)
        {
            this._transitionStaring();
            this._transitionStaringDestination();

            switch (data.Type)
            {
                case TransitionType.None:
                    break;
                case TransitionType.Size:
                    this._transitionComplete();
                    break;
                case TransitionType.Transition:
                    {
                        //return;
                        this._transitionStaringSource(data.SourceRect, data.PageSize);

                        this.TransitionValue = 0;
                        this.TransitionStoryboard.Begin();//Storyboard}
                        break;
                    }
                default:
                    break;
            }
        }
        
        #region Transition


        //Staring
        private void _transitionStaring()
        {
            this.ViewModel.Invalidate(InvalidateMode.Thumbnail);//Invalidate
        }
        private void _transitionStaringDestination()
        {
            //Transition
            Vector2 destinationPostion = this.SettingViewModel.FullScreenOffset;
            float destinationWidth = this.SettingViewModel.CenterChildWidth;
            float destinationHeight = this.SettingViewModel.CenterChildHeight;
            this.ViewModel.CanvasTransformer.TransitionDestination(destinationPostion, destinationWidth, destinationHeight);
        }
        private void _transitionStaringSource(Rect sourceRect,Size pageSize)
        {
            //Transition
            this.ViewModel.SetCanvasTransformerRadian(0.0f);
            this.ViewModel.CanvasTransformer.Transition(0.0f);
            this.ViewModel.CanvasTransformer.TransitionSource(sourceRect);
            this.ViewModel.CanvasTransformer.Size = pageSize;
        }


        //Complete
        private void _transitionComplete()
        {
            //Transition
            this.ViewModel.CanvasTransformer.Transition(1.0f);

            this.SettingViewModel.IsFullScreen = false;
            this.ViewModel.Invalidate(InvalidateMode.HD);//Invalidate
        }


        #endregion

    }
}