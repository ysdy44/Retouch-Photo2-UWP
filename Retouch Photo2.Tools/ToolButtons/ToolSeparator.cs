using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Represents the line that is used to separate tool buttons.
    /// </summary>
    public sealed partial class ToolSeparator : UserControl
    {
        //@Construct
        public ToolSeparator()
        {
            this.Content = new Rectangle
            {
                Opacity = 0.4,
                Height = 1,
                Fill = new SolidColorBrush(Colors.Gray),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
            };
        }
    }
}