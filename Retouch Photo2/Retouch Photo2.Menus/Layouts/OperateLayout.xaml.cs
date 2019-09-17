using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class OperateLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        TipViewModel TipViewModel => App.TipViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public MenuState State
        {
            set
            {
                this._Layout.State = value;
                this.OperateControl.IsOverlayExpanded = value == MenuState.OverlayExpanded;
            }
        }
        public FrameworkElement Self => this;

        public UIElement StateButton => this._Layout.StateButton;
        public UIElement CloseButton => this._Layout.CloseButton;
        public UIElement TitlePanel => this._Layout.TitlePanel;


        //@Construct
        public OperateLayout()
        {
            this.InitializeComponent();
        }
    }
}