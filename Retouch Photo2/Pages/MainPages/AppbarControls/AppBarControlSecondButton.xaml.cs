using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Pages.MainPages.AppbarControls
{
    public sealed partial class AppBarControlSecondButton : UserControl
    {
        //delegate
        public event TappedEventHandler Tap;

        //Content
        public string Glyph { get => this.FontIcon.Glyph; set => this.FontIcon.Glyph = value; }
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }

        public AppBarControlSecondButton()
        {
            this.InitializeComponent();
            this.Button.Tapped += (sender, e) => this.Tap?.Invoke(sender, e);
        }
    }
}
