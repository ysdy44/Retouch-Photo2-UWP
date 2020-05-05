using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Operates
{
    public sealed partial class ForwardOneIcon : UserControl
    {
        //@VisualState
        bool _vsIsEnabled;
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
        public ForwardOneIcon()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.IsEnabledChanged += (s, e) =>
            {
                if (e.NewValue != e.OldValue)
                {
                    this._vsIsEnabled = (bool)e.NewValue;
                    this.VisualState = this.VisualState;//State
                }
            };
        }
    }
}