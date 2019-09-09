using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class ColorMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Color;

        public override IMenuLayout Layout { get; } = new ColorLayout();
        public override IMenuButton Button { get; } = new ColorButton();
    }
}
