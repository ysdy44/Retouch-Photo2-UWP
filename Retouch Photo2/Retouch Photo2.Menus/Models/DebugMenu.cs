using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class DebugMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Debug;

        public override IMenuLayout MenuLayout { get; } = new DebugLayout();
        public override IMenuButton MenuButton { get; } = new DebugButton();
    }
}