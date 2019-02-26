using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Element.AppbarControls
{
    public sealed partial class AppBarControlSecondButton : UserControl
    {

        public string Glyph { get => this.FontIcon.Glyph; set => this.FontIcon.Glyph = value; }
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        public event TappedEventHandler Tap;

        public AppBarControlSecondButton()
        {
            this.InitializeComponent();
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)=>this.Tap(sender, e);
        
    }
}
