using Retouch_Photo2.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Layouts
{
    public sealed partial class AdjustmentLayout : UserControl, IMenuLayout
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;


        //@Content
        public MenuState State
        {
            set
            {
                this.Control.MenuTitle.State = value;
                this.Control.Height = (value == MenuState.OverlayNotExpanded) ? 40.0f : double.NaN;
            }
        }

        public FrameworkElement Self => this;

        public UIElement StateButton => this.Control.MenuTitle.StateButton;
        public UIElement CloseButton => this.Control.MenuTitle.CloseButton;
        public UIElement TitlePanel => this.Control.MenuTitle.RootGrid;


        //@Construct
        public AdjustmentLayout()
        {
            this.InitializeComponent();
        }
    }
}