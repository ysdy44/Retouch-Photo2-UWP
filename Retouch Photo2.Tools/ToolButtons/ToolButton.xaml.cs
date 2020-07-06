using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Button of <see cref="ITool"/>.
    /// </summary>
    public sealed partial class ToolButton : UserControl, IToolButton
    {

        //@Content
        /// <summary> Gets or sets the title. </summary>
        public string Title { get; set; }
        /// <summary> Gets or sets the IsSelected. </summary>
        public bool IsSelected { get => !this.Button.IsEnabled; set => this.Button.IsEnabled = !value; }
        /// <summary> Get the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Sets the center content. </summary>
        public object CenterContent { set => this.Button.Content = value; get => this.Button.Content; }
        /// <summary> Gets the ToolTip. </summary>
        public ToolTip ToolTip => this._ToolTip;

        //@Construct
        /// <summary>
        /// Initializes a ToolButton. 
        /// </summary>
        public ToolButton()
        {
            this.InitializeComponent();
        }

    }
}