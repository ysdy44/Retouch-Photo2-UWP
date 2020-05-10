using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

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
                        this.HeightBegin = ExpanderHeight.Stretch;

                        this.Closed?.Invoke(); //Delegate
                        break;

                    case ExpanderState.FlyoutShow:
                        this.HeightBegin = ExpanderHeight.Stretch;

                        this.ShowLayout();
                        if (this._vsState == ExpanderState.Hide) this.Opened?.Invoke(); //Delegate 
                        break;

                    case ExpanderState.OverlayNotExpanded:
                        this.HeightBegin = this.IsSecondPage ? ExpanderHeight.SecondToZero : ExpanderHeight.MainToZero;
                        break;

                    case ExpanderState.Overlay:
                        if (this._vsState == ExpanderState.OverlayNotExpanded)
                        {
                            this.HeightBegin = this.IsSecondPage ? ExpanderHeight.ZeroToSecond : ExpanderHeight.ZeroToMain;
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

        
        public void ShowLayout()
        {
            double flyoutPostionX = this.GetFlyoutPostionX();
            double flyoutPostionY = this.GetFlyoutPostionY();
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