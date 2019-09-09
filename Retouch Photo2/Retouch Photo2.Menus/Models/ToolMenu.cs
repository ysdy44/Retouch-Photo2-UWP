using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class ToolMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Tool;

        public override IMenuLayout MenuLayout { get; } = new ToolLayout();
        public override IMenuButton MenuButton { get; } = new ToolButton();
    }
}