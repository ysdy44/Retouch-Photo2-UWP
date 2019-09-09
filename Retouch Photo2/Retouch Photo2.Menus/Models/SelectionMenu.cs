using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class SelectionMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Selection;

        public override IMenuLayout Layout { get; } = new SelectionLayout();
        public override IMenuButton Button { get; } = new SelectionButton();
    }
}