using System;
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
          
        //@Delegate
        /// <summary> 
        /// Occurs when the position changes, Move the menu to top.
        /// </summary>
        public void Move()
        {
            if (Expander.OverlayCanvas.Children.Contains(this))
            {
                int index = Expander.OverlayCanvas.Children.IndexOf(this);
                int count = Expander.OverlayCanvas.Children.Count;
                Expander.OverlayCanvas.Children.Move((uint)index, (uint)count - 1); ;
            }
        }
        /// <summary>
        /// Occurs when the flyout opened, Disable all menus, except the current menu.
        /// </summary>
        public void Opened()
        {
            foreach (UIElement menu in Expander.OverlayCanvas.Children)
            {
                menu.IsHitTestVisible = false;
            }
            this.IsHitTestVisible = true;

            this.Move();
            this.Visibility = Visibility.Visible;
            Expander.IsOverlayDismiss = true;
        }

        /// <summary> 
        /// Occurs when the flyout closed, Enable all menus.     
        /// </summary>
        public void Closed()
        {
            foreach (UIElement menu in Expander.OverlayCanvas.Children)
            {
                menu.IsHitTestVisible = true;
            }

            this.Visibility = Visibility.Collapsed;
            Expander.IsOverlayDismiss = false;
        }

        /// <summary>
        /// Occurs when the flyout overlaid, Enable all menus.  
        /// </summary>
        public void Overlaid()
        {
            foreach (UIElement menu in Expander.OverlayCanvas.Children)
            {
                menu.IsHitTestVisible = true;
            }

            Expander.IsOverlayDismiss = false;
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
                        this.HeightStretch();
                        this.Closed(); //Delegate
                        break;

                    case ExpanderState.FlyoutShow:
                        this.HeightStretch();
                        if (this._vsState == ExpanderState.Hide) this.Opened(); //Delegate 
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
        public abstract IExpanderButton Button { get; }


        bool _lockLoad = false;
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