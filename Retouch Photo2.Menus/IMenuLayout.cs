using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    public interface IMenuLayout
    {
        MenuState State { set; }
        UIElement Self { get; }

        Flyout Flyout { get; }

        UIElement StateButton { get; }
        UIElement CloseButton { get; }
        UIElement TitlePanel { get; }
    }
}