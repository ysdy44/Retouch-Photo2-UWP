using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {
        /// <summary> Move the MenuOverlay  to a new location in the collection. </summary>
        EventHandler<UIElement> Move { get; set; }

        /// <summary> Sets IMenu's ToolTip IsOpen. </summary>
        bool IsOpen { set; }
        /// <summary> Gets IMenu's type. </summary>
        MenuType Type { get; }
        /// <summary> Gets IMenu's state. </summary>
        MenuState State { get; set; }

        /// <summary> Gets IMenu's layout. </summary>
        IMenuLayout Layout { get; }
        /// <summary> Gets IMenu's button. </summary>
        IMenuButton Button { get; }

        /// <summary> Gets overlay on canvas. </summary>
        UIElement Overlay { get; }
        /// <summary> Gets flyout. </summary>
        FlyoutBase Flyout { get; }
    }
}