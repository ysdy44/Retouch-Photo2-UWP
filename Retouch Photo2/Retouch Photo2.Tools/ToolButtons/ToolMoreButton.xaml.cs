using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Button of <see cref="ITool"/>.
    /// </summary>
    public sealed partial class ToolMoreButton : UserControl
    {
        //@Content
        /// <summary> StackPanel' Children. </summary>
        public UIElementCollection Children => this.StackPanel.Children;

        //@Construct
        /// <summary>
        /// Initializes a ToolButton. 
        /// </summary>
        public ToolMoreButton()
        {
            this.InitializeComponent();
        }
    }
}