// Core:              
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Operates
{
    /// <summary>
    /// Icon of Top.
    /// </summary>
    public sealed partial class TopIcon : UserControl
    {

        //@VisualState
        bool _vsIsEnabled;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
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
        /// <summary>
        /// Initializes a TopIcon. 
        /// </summary>
        public TopIcon()
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