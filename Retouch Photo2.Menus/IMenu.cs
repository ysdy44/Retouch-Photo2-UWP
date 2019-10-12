using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {
        /// <summary> Move the MenuOverlay to a new location in the collection. </summary>
        EventHandler<UIElement> Move { get; set; }
        /// <summary> Occurs when the "flyout" is closed. </summary>
        EventHandler<object> Closed { get; set; }
        /// <summary> Occurs when the "flyout" is opened. </summary>
        EventHandler<object> Opened { get; set; }
        
        /// <summary> Gets IMenu's type. </summary>
        MenuType Type { get; }
        /// <summary> Gets IMenu's state. </summary>
        MenuState State { get; set; }

        /// <summary> Gets IMenu's layout. </summary>
        IMenuLayout Layout { get; }
        /// <summary> Gets IMenu's button. </summary>
        IMenuButton Button { get; }
    }
}