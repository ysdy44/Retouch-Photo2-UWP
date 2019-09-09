using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu that contains a button, overlay, content, and flyout.
    /// </summary>
    public interface IMenu
    {
        /// <summary> Gets IMenu's type. </summary>
        MenuType Type { get; }
        /// <summary> Gets IMenu's state. </summary>
        MenuState State { get; set; }

        /// <summary> Gets IMenu's layout. </summary>
        IMenuLayout MenuLayout { get; }
        /// <summary> Gets IMenu's button. </summary>
        IMenuButton MenuButton { get; }
        /// <summary> Gets overlay on canvas. </summary>
        UIElement MenuOverlay { get; }
    }
}