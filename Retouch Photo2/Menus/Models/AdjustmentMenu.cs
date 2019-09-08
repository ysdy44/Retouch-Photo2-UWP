using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class AdjustmentMenu : Menu, IMenu
    {
        public MenuType Type => MenuType.Adjustment;

        public override IMenuLayout Layout { get; } = new AdjustmentLayout();
        public override IMenuButton Button { get; } = new AdjustmentButton();
    }
}