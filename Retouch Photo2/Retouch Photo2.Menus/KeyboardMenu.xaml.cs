using Retouch_Photo2.Elements;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of Keyboard.
    /// </summary>
    public sealed partial class KeyboardMenu : UserControl, IMenu
    {
        //@Construct
        /// <summary>
        /// Initializes a KeyboardMenu. 
        /// </summary>
        public KeyboardMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();
        }
    }

    /// <summary>
    /// Menu of Keyboard.
    /// </summary>
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
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Keyboard;
        /// <summary> Gets the expander. </summary>
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = "Key"
        };

        private void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}