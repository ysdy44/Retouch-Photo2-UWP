using Retouch_Photo2.Elements;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class KeyboardMenu : UserControl, IMenu
    {
        //@Construct
        public KeyboardMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();
        }
    }

    public sealed partial class KeyboardMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = "Keyboard";
        }

        //Menu
        public MenuType Type => MenuType.Keyboard;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = "Key"
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}