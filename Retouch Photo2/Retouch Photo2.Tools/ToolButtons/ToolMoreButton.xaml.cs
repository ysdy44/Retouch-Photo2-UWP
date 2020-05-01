using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class ToolMoreButton : UserControl
    {
        //@Content
        /// <summary> StackPanel' Children. </summary>
        public UIElementCollection Children => this.StackPanel.Children;

        //@Construct
        public ToolMoreButton()
        {
            this.InitializeComponent();
        }
    }
}