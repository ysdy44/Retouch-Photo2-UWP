using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            base.Content = new AppBarSeparator();
        }
    }
}