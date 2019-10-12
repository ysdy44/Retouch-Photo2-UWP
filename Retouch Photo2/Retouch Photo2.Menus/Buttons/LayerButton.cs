using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.Buttons
{
    public class LayerButton : IMenuButton
    {
        public MenuState State { set { } }
        public FrameworkElement Self { get; } = new Border();
        public MenuButtonType Type => MenuButtonType.LayersControlIndicator;
    }
}