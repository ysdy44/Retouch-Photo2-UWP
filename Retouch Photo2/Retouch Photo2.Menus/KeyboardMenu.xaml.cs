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

            this.Expander.Title = "Keyboard";//resource.GetString("/Menus/Keyboard");
        }

        //Menu
        public MenuType Type => MenuType.Debug;
        public IExpanderButton Button { get; } = new MenuButton
        {
            CenterContent = "Key"
        };
        public IExpander Expander => this._Expander;
        public ExpanderState State
        {
            set
            {
                this.Button.State = value;
                this.Expander.State = value;
            }
        }
        public FrameworkElement Self => this;

        public void ConstructMenu()
        {
            this._Expander.Button = this.Button.Self;

            this.Button.StateChanged += (state) => this.State = state;
            this.Expander.StateChanged += (state) => this.State = state;
        }

    }
}