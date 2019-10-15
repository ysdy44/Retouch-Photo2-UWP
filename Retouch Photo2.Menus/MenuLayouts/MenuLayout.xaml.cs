using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Layout of IMenu.
    /// </summary>
    public partial class MenuLayout : UserControl
    {
        //@Content
        public UIElement ContentChild { set => this.ContentBorder.Child = value; get => this.ContentBorder.Child; }
                          
        //@Construct
        public MenuLayout()
        {
            this.InitializeComponent();

            //Stop the Tapped events on OverlayCanvas in DrawPage.
            this.Tapped += (s, e) => e.Handled = true;

            //Storyboard
            this.StoryboardBorder.SizeChanged += (s, e) =>
            {
                if (this.StoryboardBorder.Visibility == Visibility.Collapsed) return;

                this.Frame.Value = e.NewSize.Height;
                this.Storyboard.Begin();
            };
        }
    }
}