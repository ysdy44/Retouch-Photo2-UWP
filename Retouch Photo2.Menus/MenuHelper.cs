using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a class that calculates the position.
    /// </summary>
    public class MenuHelper
    {

        /// <summary>
        /// Gets visual-postion in windows.
        /// </summary>
        /// <param name="element"> The element. </param>
        /// <returns> The calculated postion. </returns>
        public static Point GetVisualPostion(UIElement element) => element.TransformToVisual(Window.Current.Content).TransformPoint(new Point());


        /// <summary>
        /// Gets bound-postion in windows.
        /// </summary>
        /// <param name="postion"> The source postion. </param>
        /// <param name="element"> The source element. </param>
        /// <returns> The croped postion. </returns>
        public static Point GetBoundPostion(Point postion, FrameworkElement element)
        {
            return MenuHelper.GetBoundPostion
            (
                postion,
                element.ActualWidth,
                element.ActualHeight
            );
        }
        /// <summary>
        /// Gets bound-postion in windows.
        /// </summary>
        /// <param name="postion"> The source postion. </param>
        /// <param name="width"> The element-width. </param>
        /// <param name="height"> The element-height. </param>
        /// <returns> The croped postion. </returns>
        public static Point GetBoundPostion(Point postion, double width, double height)
        {
            double X;
            double Y;

            if (width < 222) width = 222;
            if (height < 222) height = 222;

            if (postion.X < 0) X = 0;
            else if (width > Window.Current.Bounds.Width) X = 0;
            else if (postion.X > (Window.Current.Bounds.Width - width)) X = Window.Current.Bounds.Width - width;
            else X = postion.X;

            if (postion.Y < 0) Y = 0;
            else if (height > Window.Current.Bounds.Height) Y = 0;
            else if (postion.Y > (Window.Current.Bounds.Height - height)) Y = Window.Current.Bounds.Height - height;
            else Y = postion.Y;

            return new Point(X, Y);
        }


        /// <summary>
        /// Sets overlay-postion on canvas.
        /// </summary>
        /// <param name="element"> The element on canvas. </param>
        /// <param name="postion"> The source postion. </param>
        public static void SetOverlayPostion(UIElement element, Point postion)
        {
            Canvas.SetLeft(element, postion.X);
            Canvas.SetTop(element, postion.Y);
        }
        /// <summary>
        /// Gets overlay-postion on canvas.
        /// </summary>
        /// <param name="element"> The element on canvas. </param>
        /// <returns> The calculated postion. </returns>
        public static Point GetOverlayPostion(UIElement element) => new Point(Canvas.GetLeft(element), Canvas.GetTop(element));


        /// <summary>
        /// Gets flyout-postion on canvas.
        /// </summary>
        /// <param name="button"> The source button. </param>
        /// <param name="layout"> The source layout. </param>
        /// <param name="placement"> The flyout-placement. </param>
        /// <returns> The calculated postion. </returns>
        public static Point GetFlyoutPostion(FrameworkElement button, FrameworkElement layout, FlyoutPlacementMode placement)
        {
            double layoutWidth = layout.ActualWidth;
            if (layoutWidth < 222) layoutWidth = 222;

            double layoutHeight = layout.ActualHeight;
            if (layoutHeight < 222) layoutHeight = 222;

            return MenuHelper.GetFlyoutPostion(
                MenuHelper.GetVisualPostion(button),
                button.ActualWidth, button.ActualHeight,
                layoutWidth, layoutHeight,
                placement);
        }
        /// <summary>
        /// Gets flyout-postion on canvas.
        /// </summary>
        /// <param name="buttonPostion"> The button-postion. </param>
        /// <param name="buttonWidth"> The button-width. </param>
        /// <param name="buttonHeight"> The button-height. </param>
        /// <param name="layoutWidth"> The layout-width. </param>
        /// <param name="layoutHeight"> The layout-height. </param>
        /// <param name="placement"> The flyout-placement. </param>
        /// <returns> The calculated postion. </returns>
        public static Point GetFlyoutPostion(Point buttonPostion, double buttonWidth, double buttonHeight, double layoutWidth, double layoutHeight, FlyoutPlacementMode placement)
        {
            double X = 0;
            double Y = 0;

            switch (placement)
            {
                case FlyoutPlacementMode.Top:
                case FlyoutPlacementMode.Bottom:
                    X = buttonPostion.X + buttonWidth / 2 - layoutWidth / 2;
                    break;
                case FlyoutPlacementMode.Left:
                case FlyoutPlacementMode.Right:
                    Y = buttonPostion.Y + buttonHeight / 2 - layoutHeight / 2;
                    break;
            }

            switch (placement)
            {
                case FlyoutPlacementMode.Top:
                    Y = buttonPostion.Y - layoutHeight;
                    break;
                case FlyoutPlacementMode.Bottom:
                    Y = buttonHeight;
                    break;
                case FlyoutPlacementMode.Left:
                    X = buttonPostion.X - layoutWidth;
                    break;
                case FlyoutPlacementMode.Right:
                    X = buttonPostion.X + buttonWidth;
                    break;
            }

            return new Point(X, Y);
        }
    }
}