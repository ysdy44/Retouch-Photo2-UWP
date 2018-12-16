using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Controls.LayerControls
{
    public sealed partial class LayerLayoutControl : UserControl
    {
        public UIElement Frist { get => this.FristBoder.Child; set => this.FristBoder.Child = value; }
        public UIElement Second { get => this.SecondBoder.Child; set => this.SecondBoder.Child = value; }
        public UIElement Third { get => this.ThirdBoder.Child; set => this.ThirdBoder.Child = value; }

        public LayerLayoutControl()
        {
            this.InitializeComponent();
        }
    }
}
