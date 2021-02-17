// Core:              ★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// SecondButton of <see cref="ITool"/>.
    /// </summary>
    public sealed partial class ToolSecondButton : UserControl, IToolButton
    {
        //@Content
        /// <summary> Gets the type. </summary>

        public ToolType Type
        {
            get => this.type;
            set
            {
                this.ConstructStrings(value);
                this.type = value;
            }
        }
        private ToolType type;
        /// <summary> Gets the title. </summary>
        public string Title { get; private set; }
        /// <summary> Gets or sets the IsSelected. </summary>
        public bool IsSelected { get => !this.Button.IsEnabled; set => this.Button.IsEnabled = !value; }
        /// <summary> Get the self. </summary>
        public FrameworkElement Self => this;
        /// <summary> Sets the center content. </summary>
        public object CenterContent { set => this.Button.Tag = value; get => this.Button.Tag; }
        /// <summary> Gets the ToolTip. </summary>
        public ToolTip ToolTip => null;

        //@Construct
        /// <summary>
        /// Initializes a ToolSecondButton. 
        /// </summary>
        public ToolSecondButton()
        {
            this.InitializeComponent();
        }

        //Strings
        private void ConstructStrings(ToolType type)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.Button.Content =
            this.Title = resource.GetString($"Tools_{type}");
        }

    }
}