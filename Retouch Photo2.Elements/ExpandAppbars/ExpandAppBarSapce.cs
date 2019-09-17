using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    public class ExpandAppBarSapce : FrameworkElement, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 4.0d;
        public FrameworkElement Self => this;

        //@Construct
        public ExpandAppBarSapce() => base.Width = this.ExpandWidth;
    }
}