using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    /// <summary>
    /// Menu of Debug.
    /// </summary>
    public sealed partial class DebugMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        //@Construct
        /// <summary>
        /// Initializes a DebugMenu. 
        /// </summary>
        public DebugMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();

            this.Button.Click += (s, e) =>
            {
            };
            this.Button2.Click += (s, e) =>
            {
            };

            this.Re.Click += (s, e) =>
            {
            };
            this.Coo.Click += (s, e) =>
            {
            };
        }
    }

    /// <summary>
    /// Menu of Debug.
    /// </summary>
    public sealed partial class DebugMenu : UserControl, IMenu
    {

        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = "Debug"; //resource.GetString("/Menus/Debug");
        }

        //Menu
        /// <summary> Gets the type. </summary>
        public MenuType Type => MenuType.Debug;
        /// <summary> Gets the expander. </summary>
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = "?"
        };

        private void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}