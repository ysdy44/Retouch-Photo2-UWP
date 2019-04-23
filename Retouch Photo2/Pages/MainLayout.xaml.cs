using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages
{
    public sealed partial class MainLayout : UserControl
    {
        //Content
        public UIElement Head { get => this.HeadBorder.Child; set => this.HeadBorder.Child = value; }
        public UIElement Body { get => this.BodyBorder.Child; set => this.BodyBorder.Child = value; }
        public UIElement Foot { get => this.FootGrid.Child; set => this.FootGrid.Child = value; }

        public MainLayout()
        {
            this.InitializeComponent();

            // Appbar
            this.FootGrid.SizeChanged += (sender, e) =>
            {
                this.AppbarRectangleFrameWidth.Value = e.NewSize.Width;
                this.AppbarRectangleStoryboard.Begin();
            };
        }
    }
}
