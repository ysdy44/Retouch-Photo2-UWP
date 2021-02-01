// Core:              
// Referenced:   ★
// Difficult:         
// Only:              
// Complete:      
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Separator of <see cref="ExpandAppbar"/>.
    /// </summary>
    public class ExpandAppBarSeparator : UserControl, IExpandAppbarElement
    {
        //@Content
        public double ExpandWidth => 32.0d;
        public FrameworkElement Self => this;
        public bool IsSecondPage
        {
            set
            {
                if (value==false)
                {
                    this.Width = this.ExpandWidth;

                    this._rectangle.Width = 1;
                    this._rectangle.Height = 26;
                    this._rectangle.HorizontalAlignment = HorizontalAlignment.Center;
                }
                else
                {
                    this.Width = double.NaN;

                    this._rectangle.Width = double.NaN;
                    this._rectangle.Height = 1;
                    this._rectangle.HorizontalAlignment = HorizontalAlignment.Stretch;
                }
            }
        }

        Rectangle _rectangle = new Rectangle
        {
            Width = 1,
            Opacity = 0.4,
            Height = 26,
            Fill = new SolidColorBrush(Colors.Gray),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        //@Construct
        /// <summary>
        /// Initializes a ExpandAppBarSeparator.
        /// </summary>
        public ExpandAppBarSeparator() : base()
        {
            base.Width = this.ExpandWidth;
            base.Content = this._rectangle;
        }
    }
}