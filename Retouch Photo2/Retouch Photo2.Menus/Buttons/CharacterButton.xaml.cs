using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.ViewModels;

namespace Retouch_Photo2.Menus.Buttons
{
    public sealed partial class CharacterButton : UserControl, IMenuButton
    {
        //@ViewModel
        TipViewModel TipViewModel => App.TipViewModel;

        //@Content
        public MenuState State { set => this.Button.MenuState = value; }
        public FrameworkElement Self => this;
        public MenuButtonType Type => MenuButtonType.None;

        //@Construct
        public CharacterButton()
        {
            this.InitializeComponent();
        }
    }
}