// Core:              
// Referenced:   ★
// Difficult:         
// Only:              
// Complete:      
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Sapce of <see cref="ExpandAppbar"/>.
    /// </summary>
    public class ExpandAppBarSapce : FrameworkElement, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 4.0d;
        public FrameworkElement Self => this;
        public bool IsSecondPage { set => this.Visibility = value ? Visibility.Collapsed : Visibility.Visible; }

        //@Construct
        /// <summary>
        /// Initializes a ExpandAppBarSapce.
        /// </summary>
        public ExpandAppBarSapce() : base()
        {
            base.Width = this.ExpandWidth;
        }
    }
}