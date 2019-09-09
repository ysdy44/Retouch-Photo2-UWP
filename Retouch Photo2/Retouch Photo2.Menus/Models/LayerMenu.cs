using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class LayerMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Layer;

        public override IMenuLayout Layout { get; } = new LayerLayout();
        public override IMenuButton Button { get; } = new LayerButton();
    }
}
