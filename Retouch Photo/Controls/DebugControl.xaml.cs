using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls
{
    public sealed partial class DebugControl : Page
    {
        public DebugControl()
        {
            this.InitializeComponent();
            this.Button.Tapped += (sender, e) => this.RequestedTheme = this.RequestedTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
        }
    }
}
