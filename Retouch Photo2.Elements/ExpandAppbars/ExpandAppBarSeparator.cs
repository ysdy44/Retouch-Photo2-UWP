using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public class ExpandAppBarSeparator : AppBarSeparator, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 4.0d;
        public FrameworkElement Self => this;
    }
}