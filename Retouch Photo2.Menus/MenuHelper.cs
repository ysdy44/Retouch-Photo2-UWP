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
        /// Construct menus.
        /// </summary>
        /// <param name="menus"> The source menus. </param>
        /// <param name="overlayCanvas"> The overlay-canvas. </param>
        /// <param name="headChildren"> The head-children. </param>
        /// <param name="indicatorBorder"> The indicator-border. </param>
        public static void ConstructMenus(IList<IMenu> menus, Canvas overlayCanvas, UIElementCollection headChildren, Border indicatorBorder)
        {
            foreach (IMenu menu in menus)
            {
                if (menu == null) continue;

                FrameworkElement layout = menu.Layout.Self;
                overlayCanvas.Children.Add(layout);

                //Move to top
                menu.Move += (s, e) =>
                {
                    int index = overlayCanvas.Children.IndexOf(menu.Layout.Self);
                    int count = overlayCanvas.Children.Count;
                    overlayCanvas.Children.Move((uint)index, (uint)count - 1);
                };
                //Menus is disable
                menu.Opened += (s, e) =>
                {
                    foreach (IMenu other in menus)
                    {
                        if (other != menu)
                        {
                            other.Layout.Self.IsHitTestVisible = false;
                        }
                    }
                    overlayCanvas.Background = new SolidColorBrush(Colors.Transparent);
                };
                //Menus is enable
                menu.Closed += (s, e) =>
                {
                    foreach (IMenu other in menus)
                    {
                        other.Layout.Self.IsHitTestVisible = true;
                    }
                    overlayCanvas.Background = null;
                };

                //MenuButton
                IMenuButton menuButton = menu.Button;
                switch (menuButton.Type)
                {
                    case MenuButtonType.None:
                        headChildren.Add(menuButton.Self);
                        menu.Button.Self.Tapped += (s, e) => menu.State = MenuHelper.GetState(menu.State);
                        break;
                    case MenuButtonType.LayersControlIndicator:
                        indicatorBorder.Child = menuButton.Self;
                        break;
                }
            }

            overlayCanvas.Tapped += (s, e) => menusHide();
            overlayCanvas.SizeChanged += (s, e) => menusHide();

            void menusHide()
            {
                foreach (IMenu other in menus)
                {
                    if (other.State == MenuState.FlyoutShow)
                    {
                        other.State = MenuState.FlyoutHide;
                    }
                }
                overlayCanvas.Background = null;
            }
        }


        /// <summary>
        /// Get the corresponding status. 
        /// </summary>
        /// <param name="state"> The source state. </param>
        /// <returns> The destination state. </returns>
        public static MenuState GetState(MenuState state)
        {
            switch (state)
            {
                case MenuState.FlyoutHide:
                    return MenuState.FlyoutShow;
                case MenuState.FlyoutShow:
                    return MenuState.FlyoutHide;

                case MenuState.OverlayExpanded:
                    return MenuState.OverlayNotExpanded;
                case MenuState.OverlayNotExpanded:
                    return MenuState.OverlayExpanded;
            }
            return MenuState.FlyoutShow;
        }


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
            if (layoutWidth < 200) layoutWidth = 200;

            double layoutHeight = layout.ActualHeight;
            if (layoutHeight < 200) layoutHeight = 200;

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