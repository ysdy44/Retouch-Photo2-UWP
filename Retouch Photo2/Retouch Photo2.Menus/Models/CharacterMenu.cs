using Retouch_Photo2.Menus.Buttons;
using Retouch_Photo2.Menus.Layouts;

namespace Retouch_Photo2.Menus.Models
{
    public class CharacterMenu : MenuBase, IMenu
    {
        public MenuType Type => MenuType.Character;

        public override IMenuLayout Layout { get; } = new CharacterLayout();
        public override IMenuButton Button { get; } = new CharacterButton();
    }
}