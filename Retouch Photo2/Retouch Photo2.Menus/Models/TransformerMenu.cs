using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class TransformerMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Transformer;

        public override IMenuLayout Layout { get; } = new TransformerLayout();
        public override IMenuButton Button { get; } = new TransformerButton();
    }
}