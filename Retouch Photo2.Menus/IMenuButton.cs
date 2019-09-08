using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    public interface IMenuButton
    {
        MenuState State { set; }
        FrameworkElement Self { get; }
    }
}