using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Buttons
{
    public sealed partial class ToolButton : UserControl, IMenuButton
    {
        //@Content
        public MenuState State
        {
            set
            {
                switch (value)
                {
                    case MenuState.FlyoutHide:
                        this.Visibility = Visibility.Visible;
                        break;
                    case MenuState.FlyoutShow:
                        this.Visibility = Visibility.Visible;
                        break;
                    case MenuState.RootExpanded:
                        this.Visibility = Visibility.Collapsed;
                        break;
                    case MenuState.RootNotExpanded:
                        this.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
        public FrameworkElement Self => this;
        public MenuButtonType Type => MenuButtonType.ToolButton;

        //@Construct
        public ToolButton()
        {
            this.InitializeComponent();
        }
    }
}