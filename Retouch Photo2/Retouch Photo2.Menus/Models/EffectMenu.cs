using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class EffectMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Effect;

        public override IMenuLayout Layout { get; } = new EffectLayout();
        public override IMenuButton Button { get; } = new EffectButton();
    }
}
