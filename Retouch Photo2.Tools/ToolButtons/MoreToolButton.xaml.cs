using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's more Button.
    /// </summary>
    public sealed partial class MoreToolButton : UserControl
    {
        //@Construct
        public MoreToolButton()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Add UIElement to StackPanel Children.
        /// </summary>
        /// <param name="element"></param>
        public void Add(UIElement element)
        {
            this.StackPanel.Children.Add(element);
        }
    }
}
