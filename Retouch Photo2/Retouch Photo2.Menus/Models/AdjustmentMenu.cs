using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class AdjustmentMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Adjustment;

        public override IMenuLayout MenuLayout { get; } = new AdjustmentLayout();
        public override IMenuButton MenuButton { get; } = new AdjustmentButton();
    }
}