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
    /// Button of <see cref="ITool"/>.
    /// </summary>
    public sealed partial class ToolButton : UserControl, IToolButton
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
        /// <summary> Sets the icon. </summary>
        public object Icon { set => this.Button.Content = value; get => this.Button.Content; }
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

        //Strings
        private void ConstructStrings(ToolType type)
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            this.ToolTip.Content =
            this.Title = resource.GetString($"Tools_{type}");
        }

    }
}