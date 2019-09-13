using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Menus.Buttons
{
    public sealed partial class ToolButton : IMenuButton
    {
        //@Content
        public MenuState State
        {
            set
            {
                switch (value)
                {
                    case MenuState.FlyoutHide:
                    case MenuState.FlyoutShow:
                        this.Self.Visibility = Visibility.Visible;
                        break;
                    case MenuState.OverlayExpanded:
                    case MenuState.OverlayNotExpanded:
                        this.Self.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
        public FrameworkElement Self{ get; } = new Button
        {
            Height = 50,
            BorderThickness = new Thickness(0),
            BorderBrush = new SolidColorBrush(Colors.Transparent),
            Background = new SolidColorBrush(Colors.Transparent),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Content = new TextBlock
            {
                Text = ". . .",
                VerticalAlignment = VerticalAlignment.Center,
            }
        };
        public MenuButtonType Type => MenuButtonType.ToolButton;
    }
}