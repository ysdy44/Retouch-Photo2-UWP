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
            //ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._Expander.Title = "Keyboard";//resource.GetString("/Menus/Keyboard");
        }

        //Menu
        public MenuType Type => MenuType.Debug;
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