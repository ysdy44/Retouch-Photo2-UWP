using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class DebugMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Debug;

        public override IMenuLayout Layout { get; } = new DebugLayout();
        public override IMenuButton Button { get; } = new DebugButton();
    }
}