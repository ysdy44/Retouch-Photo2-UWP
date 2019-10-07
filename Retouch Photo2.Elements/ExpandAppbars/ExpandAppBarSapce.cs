using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    public class ExpandAppBarSapce : FrameworkElement, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 4.0d;
        public FrameworkElement Self => this;
        public bool IsSecondPage { set => this.Visibility = value ? Visibility.Collapsed : Visibility.Visible; }

        //@Construct
        public ExpandAppBarSapce() : base()
        {
            base.Width = this.ExpandWidth;
        }
    }
}