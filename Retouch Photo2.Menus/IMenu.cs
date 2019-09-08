using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    public interface IMenu
    {
        MenuType Type { get; }
        MenuState State { get; set; }

        /// <summary>
        /// 提供 Flyout，并在 Flyout 和 Content 里自由穿梭
        /// </summary>
        IMenuLayout Layout { get; }

        /// <summary>
        /// Topbar
        /// </summary>
        IMenuButton Button { get; }

        /// <summary>
        /// Canvas
        /// </summary>
        Border Content { get; }
    }
}