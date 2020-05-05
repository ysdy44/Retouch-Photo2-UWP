using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Operates
{
    public sealed partial class RightIcon : UserControl
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
        public RightIcon()
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