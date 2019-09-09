using Retouch_Photo2.ViewModels;
using Retouch_Photo2.ViewModels.Selections;
using Retouch_Photo2.ViewModels.Tips;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Buttons
{
    public sealed partial class ColorButton : UserControl, IMenuButton
    {
        //@ViewModel
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;

        //@Content
        public MenuState State { set { } }
        public FrameworkElement Self => this;
        public MenuButtonType Type => MenuButtonType.None;

        //@Construct
        public ColorButton()
        {
            this.InitializeComponent();
        }
    }
}