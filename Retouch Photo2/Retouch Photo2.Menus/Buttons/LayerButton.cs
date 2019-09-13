using Retouch_Photo2.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Menus.Buttons
{
    public class LayerButton : IMenuButton
    {
        public MenuState State { set { } }
        public FrameworkElement Self{ get; } = new LayersControl();
        public MenuButtonType Type => MenuButtonType.LayersControl;
    }
}