using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class OperateMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Operate;

        public override IMenuLayout Layout { get; } = new OperateLayout();
        public override IMenuButton Button { get; } = new OperateButton();
    }
}
