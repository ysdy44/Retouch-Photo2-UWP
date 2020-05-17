using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public partial class Expander : UserControl, IExpander
    {

        //@Delegate
        public Action Move { get; set; }
        public Action Closed { get; set; }
        public Action Opened { get; set; }
        public Action Overlaid { get; set; }


        //@Content
        public ExpanderState State
        {
            get => this._vsState;
            set
            {
                switch (value)
                {
                    case ExpanderState.Hide:
                        this.HeightRectangle.VerticalAlignment = VerticalAlignment.Stretch;
                        this.HeightRectangle.Height = double.NaN;

                        this.Closed?.Invoke(); //Delegate
                        break;

                    case ExpanderState.FlyoutShow:
                        this.HeightRectangle.VerticalAlignment = VerticalAlignment.Stretch;
                        this.HeightRectangle.Height = double.NaN;

                        if (this._vsState == ExpanderState.Hide) this.Opened?.Invoke(); //Delegate 
                        break;

                    case ExpanderState.OverlayNotExpanded:
                        this.HeightRectangle.VerticalAlignment = VerticalAlignment.Top;
                        if (this.IsSecondPage) this.HeightStoryboardSecondToZero.Begin();//Storyboard
                        else this.HeightStoryboardMainToZero.Begin();//Storyboard
                        break;

                    case ExpanderState.Overlay:
                        if (this._vsState == ExpanderState.OverlayNotExpanded)
                        {
                            this.HeightRectangle.VerticalAlignment = VerticalAlignment.Top;
                            if (this.IsSecondPage) this.HeightStoryboardZeroToSecond.Begin();//Storyboard
                            else this.HeightStoryboardZeroToMain.Begin();//Storyboard
                        }

                        this.Overlaid?.Invoke(); //Delegate 
                        break;

                    default:
                        break;
                }

                this.Button.State = value;
                this._vsState = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public FrameworkElement Layout { get; set; }
        public IExpanderButton Button { get; set; }

        bool _lockLoad = false;
        public void CalculatePostion(FrameworkElement placementTarget, FlyoutPlacementMode placementMode)
        {
            //@Debug: 
            // if unloaded, the height will < 70
            // so it must re-Load and re-CalculatePostion
            if (this._lockLoad == false && this.Layout.ActualHeight < 70)
            {
                this.Loaded += (s, e) =>
                {
                    this._lockLoad = true;
                    this.CalculatePostion(placementTarget, placementMode);
                };
                return;
            }

            //Gets visual-postion in windows.
            Point buttonPostion = placementTarget.TransformToVisual(Window.Current.Content).TransformPoint(new Point());//@VisualPostion
            double flyoutPostionX = this.GetFlyoutPostionX(buttonPostion.X, placementTarget.ActualWidth, placementMode);
            double flyoutPostionY = this.GetFlyoutPostionY(buttonPostion.Y, placementTarget.ActualHeight, placementMode);

            this.PostionX = this.GetBoundPostionX(flyoutPostionX);
            this.PostionY = this.GetBoundPostionY(flyoutPostionY);
        }

        public void HideLayout()
        {
            switch (this.State)
            {
                case ExpanderState.FlyoutShow:
                    this.State = ExpanderState.Hide;
                    break;
            }

            this.Layout.IsHitTestVisible = true;
        }

        public void CropLayout()
        {
            switch (this.State)
            {
                case ExpanderState.FlyoutShow:
                    this.State = ExpanderState.Hide;
                    break;

                case ExpanderState.Overlay:
                case ExpanderState.OverlayNotExpanded:
                    this.PostionX = this.GetBoundPostionX(this.PostionX);
                    this.PostionY = this.GetBoundPostionY(this.PostionY);
                    break;
            }
        }

    }
}