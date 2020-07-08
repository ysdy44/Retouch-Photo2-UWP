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
        /// Occurs when the position changes, Move the menu to top.
        /// </summary>
        private void Move()
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
        private void Opened()
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
        private void Closed()
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
        private void Overlaid()
        {
            foreach (UIElement menu in Expander.OverlayCanvas.Children)
            {
                menu.IsHitTestVisible = true;
            }

            Expander.IsOverlayDismiss = false;
        }
               


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
        
    }
}