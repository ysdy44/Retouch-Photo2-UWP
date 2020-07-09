using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Elements
{
    /// <summary> 
    /// Represents the control that a drawer can be folded.
    /// </summary>
    public abstract partial class Expander : UserControl
    {

        //@Content
        public string Title { get => this.TitleTextBlock.Text; protected set => this.TitleTextBlock.Text = value; }
        public UIElement Page { get => this.PageBorder.Child; set => this.PageBorder.Child = value; }
        public abstract UIElement MainPage { get; }
        public bool IsSecondPage
        {
            get => this._vsIsSecondPage;
            set
            {
                if (this._vsIsSecondPage != value)
                {
                    if (value) this.TitleShowStoryboard.Begin();//Storyboard
                    else this.TitleFadeStoryboard.Begin();//Storyboard
                }

                this._vsIsSecondPage = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public Visibility ResetButtonVisibility { get => this.ResetButton.Visibility; set => this.ResetButton.Visibility = value; }
        public abstract void Reset();
        public void Back()
        {
            this.Title = this.Button.Title;
            this.Page = this.MainPage;
            this.IsSecondPage = false;
        }


        //@Content
        public ExpanderState State
        {
            get => this._vsState;
            set
            {
                switch (value)
                {
                    case ExpanderState.Hide:
                        this.Closed(); //Delegate
                        break;

                    case ExpanderState.FlyoutShow:
                        if (this._vsState == ExpanderState.Hide) this.Opened(); //Delegate 
                        break;

                    case ExpanderState.OverlayNotExpanded:
                        this.HeightStoryboard.Begin();//Storyboard
                        break;

                    case ExpanderState.Overlay:
                        if (this._vsState == ExpanderState.OverlayNotExpanded)
                        {
                            this.HeightStoryboard.Begin();//Storyboard
                        }

                        this.Overlaid(); //Delegate 
                        break;

                    default:
                        break;
                }

                this.Button.ExpanderState = value;
                this._vsState = value;
                this.VisualState = this.VisualState; //State
            }
        }
        public FlyoutPlacementMode PlacementMode { get; set; } = FlyoutPlacementMode.Bottom;
        public FrameworkElement Self => this;
        public abstract IExpanderButton Button { get; }


        //@Content
        bool _lockLoad = false;
        double _postionX;
        double _postionY;
        public double PostionX { get => Canvas.GetLeft(this); set => Canvas.SetLeft(this, value); }
        public double PostionY { get => Canvas.GetTop(this); set => Canvas.SetTop(this, value); }

        public void CalculatePostion(FrameworkElement placementTarget, FlyoutPlacementMode placementMode)
        {
            //@Debug: 
            // if unloaded, the height will < 70
            // so it must re-Load and re-CalculatePostion
            if (this._lockLoad == false && this.ActualHeight < 70)
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

            this.IsHitTestVisible = true;
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