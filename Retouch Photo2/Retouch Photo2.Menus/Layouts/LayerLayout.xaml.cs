using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class LayerLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content
        public MenuState State
        {
            set
            {
                this.Control.MenuTitle.State = value;
                this.Control.Height = (value == MenuState.OverlayNotExpanded) ? 40.0f : double.NaN;
                this.Control.IsOverlayExpanded = value == MenuState.OverlayExpanded;
            }
        }
        public FrameworkElement Self => this;

        public UIElement StateButton => this.Control.MenuTitle.StateButton;
        public UIElement CloseButton => this.Control.MenuTitle.CloseButton;
        public UIElement TitlePanel => this.Control.MenuTitle.RootGrid;


        //@Construct
        public LayerLayout()
        {
            this.InitializeComponent();
        }
    }
}