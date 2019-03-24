using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Effects
{
    public sealed partial class Control : UserControl
    {
        public UIElement Icon { get; set; }

        //Content
        public Button Button { get => this.RootButton; set => this.RootButton = value; }
        public ToggleSwitch ToggleSwitch { get => this.RootToggleSwitch; set => this.RootToggleSwitch = value; }
        
        public Control()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.RootButton.Content = this.Icon;
        }
    }
}

