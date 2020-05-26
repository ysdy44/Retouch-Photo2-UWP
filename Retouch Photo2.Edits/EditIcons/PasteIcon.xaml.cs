using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Edits.EditIcons
{
    public sealed partial class PasteIcon : UserControl
    {
        //@VisualState
        bool _vsIsEnabled => this.IsEnabled;
        public VisualState VisualState
        {
            get
            {
                if (this.IsEnabled) return this.Normal;
                else return this.Disabled;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        //@Construct
        public PasteIcon()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.IsEnabledChanged += (s, e) =>
            {
                if (e.NewValue != e.OldValue)
                {
                    this.VisualState = this.VisualState;//State
                }
            };
        }
    }
}