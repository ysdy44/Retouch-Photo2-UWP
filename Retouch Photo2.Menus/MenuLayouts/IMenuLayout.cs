using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus
{
    public interface IMenuLayout
    {
        /// <summary> Sets IMenuLayout's state. </summary>
        MenuState State { set; }
        /// <summary> Gets it yourself. </summary>
        FrameworkElement Self { get; }

        /// <summary> Gets layout's StateButton. </summary>
        UIElement StateButton { get; }
        /// <summary> Gets layout's CloseButton. </summary>
        UIElement CloseButton { get; }
        /// <summary> Gets layout's TitlePanel. </summary>
        UIElement TitlePanel { get; }
    }
}