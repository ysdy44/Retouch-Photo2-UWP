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

        /// <summary>
        /// Gets flyout-postion X on canvas.
        /// </summary>
        private double GetFlyoutPostionX(double buttonPostionX, double buttonWidth, FlyoutPlacementMode placementMode)
        {
            double layoutWidth = this.ActualWidth;
            if (layoutWidth < 222) layoutWidth = 222;

            switch (placementMode)
            {
                case FlyoutPlacementMode.Top:
                case FlyoutPlacementMode.Bottom:
                    return buttonPostionX + buttonWidth / 2 - layoutWidth / 2;
                case FlyoutPlacementMode.Left:
                    return buttonPostionX - layoutWidth;
                case FlyoutPlacementMode.Right:
                    return buttonPostionX + buttonWidth;
                default: return 0;
            }
        }
        /// <summary>
        /// Gets flyout-postion Y on canvas.
        /// </summary>
        private double GetFlyoutPostionY(double buttonPostionY, double buttonHeight, FlyoutPlacementMode placementMode)
        {
            double layoutHeight = this.ActualHeight;
            if (layoutHeight < 50) layoutHeight = 50;

            switch (placementMode)
            {
                case FlyoutPlacementMode.Top:
                    return buttonPostionY - layoutHeight;
                case FlyoutPlacementMode.Bottom:
                    return buttonHeight;
                case FlyoutPlacementMode.Left:
                case FlyoutPlacementMode.Right:
                    return buttonPostionY + buttonHeight / 2 - layoutHeight / 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Gets bound-postion X in windows.
        /// </summary>
        /// <param name="postionX"> The source postion X. </param>
        /// <returns> The croped postion. </returns>
        private double GetBoundPostionX(double postionX)
        {
            double width = this.ActualWidth;

            if (width < 222) width = 222;

            if (postionX < 0) postionX = 0;
            else if (width > Window.Current.Bounds.Width) postionX = 0;
            else if (postionX > (Window.Current.Bounds.Width - width)) postionX = Window.Current.Bounds.Width - width;

            return postionX;
        }
        /// <summary>
        /// Gets bound-postion Y in windows.
        /// </summary>
        /// <param name="postionY"> The source postion Y. </param>
        /// <returns> The croped postion. </returns>
        private double GetBoundPostionY(double postionY)
        {
            double height = this.ActualHeight;

            if (height < 40) height = 40;

            if (postionY < 0) postionY = 0;
            else if (height > Window.Current.Bounds.Height) postionY = 0;
            else if (postionY > (Window.Current.Bounds.Height - height)) postionY = Window.Current.Bounds.Height - height;

            return postionY;
        }


        private ExpanderState GetState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.Overlay;
        }
        private ExpanderState GetButtonState(ExpanderState state)
        {
            switch (state)
            {
                case ExpanderState.Hide: return ExpanderState.FlyoutShow;
                case ExpanderState.FlyoutShow: return ExpanderState.Hide;

                case ExpanderState.Overlay: return ExpanderState.OverlayNotExpanded;
                case ExpanderState.OverlayNotExpanded: return ExpanderState.Overlay;
            }
            return ExpanderState.FlyoutShow;
        }

    }
}