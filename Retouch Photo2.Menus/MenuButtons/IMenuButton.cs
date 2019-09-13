using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Represents a menu button.
    /// </summary>
    public interface IMenuButton
    {
        /// <summary> Sets IMenuButton's state. </summary>
        MenuState State { set; }
        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }
        /// <summary> Gets IMenuButton's type. </summary>
        MenuButtonType Type { get; }
    }
}