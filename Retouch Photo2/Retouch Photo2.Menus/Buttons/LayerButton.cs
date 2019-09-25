using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus.Buttons
{
    public class LayerButton : IMenuButton
    {
        public MenuState State { set { } }
        public FrameworkElement Self { get; } = true ? new Border() : new Border
        {
            Background = new SolidColorBrush(Colors.Red),
            Child=new TextBlock
            {
                Text = "Indicator",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            }
        };
        public MenuButtonType Type => MenuButtonType.LayersControlIndicator;
    }
}