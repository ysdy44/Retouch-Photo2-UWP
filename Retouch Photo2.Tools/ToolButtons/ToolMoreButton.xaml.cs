using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Moew button of <see cref="ITool"/>.
    /// </summary>
    public sealed partial class ToolMoreButton : UserControl, IToolButton
    {
        //@Content
        /// <summary> StackPanel' Children. </summary>
        public UIElementCollection Children => this.StackPanel.Children;

        //@Content
        /// <summary> Gets or sets the title. </summary>
        public string Title { get; set; }
        /// <summary> Gets or sets the IsSelected. </summary>
        public bool IsSelected { get; set; }
        /// <summary> Get the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Sets the center content. </summary>
        public object CenterContent { set; get; }
        /// <summary> Gets the ToolTip. </summary>
        public ToolTip ToolTip => null;

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