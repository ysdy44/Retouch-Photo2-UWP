using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class HistoryMenu : UserControl, IMenu
    { 
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;

        //@Construct
        public HistoryMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();
            
            this.ListView.ItemsSource = this.ViewModel.Historys;
        }
    }

    public sealed partial class HistoryMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = resource.GetString("/Menus/History");
        }

        //Menu
        public MenuType Type => MenuType.History;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Historys.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}