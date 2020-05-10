using Windows.Foundation;
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

        /// <summary>
        /// Gets visual-postion in windows.
        /// </summary>
        /// <param name="element"> The element. </param>
        /// <returns> The calculated postion. </returns>
        private Point GetVisualPostion(UIElement element) => element.TransformToVisual(Window.Current.Content).TransformPoint(new Point());

        /// <summary>
        /// Gets flyout-postion X on canvas.
        /// </summary>
        private double GetFlyoutPostionX()
        {
            double layoutWidth = this.Layout.ActualWidth;
            if (layoutWidth < 222) layoutWidth = 222;

            Point buttonPostion = this.GetVisualPostion(this.Button.Self);
            double buttonWidth = this.Button.Self.ActualWidth;

            switch (this.PlacementMode)
            {
                case FlyoutPlacementMode.Top:
                case FlyoutPlacementMode.Bottom:
                    return buttonPostion.X + buttonWidth / 2 - layoutWidth / 2;
                case FlyoutPlacementMode.Left:
                    return buttonPostion.X - layoutWidth;
                case FlyoutPlacementMode.Right:
                    return buttonPostion.X + buttonWidth;
                default: return 0;
            }
        }
        /// <summary>
        /// Gets flyout-postion Y on canvas.
        /// </summary>
        private double GetFlyoutPostionY()
        {
            double layoutHeight = this.Layout.ActualHeight;
            if (layoutHeight < 50) layoutHeight = 50;

            Point buttonPostion = this.GetVisualPostion(this.Button.Self);
            double buttonHeight = this.Button.Self.ActualHeight;

            switch (this.PlacementMode)
            {
                case FlyoutPlacementMode.Top:
                    return buttonPostion.Y - layoutHeight;
                case FlyoutPlacementMode.Bottom:
                    return buttonHeight;
                case FlyoutPlacementMode.Left:
                case FlyoutPlacementMode.Right:
                    return buttonPostion.Y + buttonHeight / 2 - layoutHeight / 2;
                default: return 0;
            }
        }

        /// <summary>
        /// Gets bound-postion X in windows.
        /// </summary>
        /// <param name="postionX"> The source postion X. </param>
        /// <param name="element"> The source element. </param>
        /// <returns> The croped postion. </returns>
        private double GetBoundPostionX(double postionX)
        {
            double width = this.Layout.ActualWidth;

            if (width < 222) width = 222;

            if (postionX < 0) postionX = 0;
            else if (width > Window.Current.Bounds.Width) postionX = 0;
            else if (postionX > (Window.Current.Bounds.Width - width)) postionX = Window.Current.Bounds.Width - width;

            return postionX;
        }
        /// <summary>
        /// Gets bound-postion Y in windows.
        /// </summary>
        /// <param name="postion"> The source postion. </param>
        /// <param name="element"> The source element. </param>
        /// <returns> The croped postion. </returns>
        private double GetBoundPostionY(double postionY)
        {
            double height = this.Layout.ActualHeight;

            if (height < 40) height = 40;

            if (postionY < 0) postionY = 0;
            else if (height > Window.Current.Bounds.Height) postionY = 0;
            else if (postionY > (Window.Current.Bounds.Height - height)) postionY = Window.Current.Bounds.Height - height;

            return postionY;
        }

    }
}