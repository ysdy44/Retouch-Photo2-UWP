using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Elements
{
    public class ExpandAppBarSeparator : UserControl, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 32.0d;
        public FrameworkElement Self => this;

        //@Construct
        public ExpandAppBarSeparator() : base()
        {
            base.Width = this.ExpandWidth;
            base.Content = new Rectangle
            {
                Width = 1,
                Opacity = 0.6,
                Height = 26,
                Fill = new SolidColorBrush(Colors.Gray),
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }
    }
}