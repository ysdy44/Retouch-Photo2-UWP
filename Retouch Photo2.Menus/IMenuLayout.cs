using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    public interface IMenuLayout
    {
        /// <summary> Sets IMenuLayout's ToolTip IsOpen. </summary>
        bool IsOpen { set; }
        /// <summary> Sets IMenuLayout's state. </summary>
        MenuState State { set; }
        /// <summary> Gets it yourself. </summary>
        UIElement Self { get; }

        /// <summary> Gets flyout. </summary>
        Flyout Flyout { get; }

        /// <summary> Gets layout's StateButton. </summary>
        UIElement StateButton { get; }
        /// <summary> Gets layout's CloseButton. </summary>
        UIElement CloseButton { get; }
        /// <summary> Gets layout's TitlePanel. </summary>
        UIElement TitlePanel { get; }
    }
}