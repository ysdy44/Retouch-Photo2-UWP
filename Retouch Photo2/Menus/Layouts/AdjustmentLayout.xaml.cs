using Retouch_Photo2.ViewModels.Selections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class AdjustmentLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public MenuState State { set => this._MenuLayout.State = value; }
        public UIElement Self => this;

        public Flyout Flyout => this._Flyout;

        public UIElement StateButton => this._MenuLayout.StateButton;
        public UIElement CloseButton => this._MenuLayout.CloseButton;
        public UIElement TitlePanel => this._MenuLayout.TitlePanel;


        //@Construct
        public AdjustmentLayout()
        {
            this.InitializeComponent();
        }
    }
}