using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class EffectMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Effect;

        public override IMenuLayout MenuLayout { get; } = new EffectLayout();
        public override IMenuButton MenuButton { get; } = new EffectButton();
    }
}
