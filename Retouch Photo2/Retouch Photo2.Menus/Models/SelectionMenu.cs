using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class SelectionMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Selection;

        public override IMenuLayout MenuLayout { get; } = new SelectionLayout();
        public override IMenuButton MenuButton { get; } = new SelectionButton();
    }
}