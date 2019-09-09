using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class TransformerMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Transformer;

        public override IMenuLayout MenuLayout { get; } = new TransformerLayout();
        public override IMenuButton MenuButton { get; } = new TransformerButton();
    }
}