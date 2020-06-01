using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Models
{
    public sealed partial class ParagraphMenu : UserControl, IMenu
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;

        //@Construct
        public ParagraphMenu()
        {
            this.InitializeComponent();
            this.ConstructStrings();
            this.ConstructMenu();
        }
    }

    public sealed partial class ParagraphMenu : UserControl, IMenu
    {
        //Strings
        private void ConstructStrings()
        {
            this._button.ToolTip.Content =
            this._Expander.Title =
            this._Expander.CurrentTitle = "Paragraph";
        }

        //Menu
        public MenuType Type => MenuType.Paragraph;
        public IExpander Expander => this._Expander;
        MenuButton _button = new MenuButton
        {
            CenterContent = new Retouch_Photo2.Characters.Icon()
        };

        public void ConstructMenu()
        {
            this._Expander.Layout = this;
            this._Expander.Button = this._button;
            this._Expander.Initialize();
        }
    }
}