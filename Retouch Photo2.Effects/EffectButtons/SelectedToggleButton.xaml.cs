// Core:              
// Referenced:   
// Difficult:         
// Only:              
// Complete:      
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Button of <see cref="Effect "/>.
    /// </summary>
    public sealed partial class SelectedToggleButton : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets the state. </summary>
        public bool IsChecked
        {
            get => (bool)base.GetValue(IsCheckedProperty);
            set => base.SetValue(IsCheckedProperty, value);
        }
        /// <summary> Identifies the <see cref = "SelectedToggleButton.IsChecked" /> dependency property. </summary>
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(SelectedToggleButton), new PropertyMetadata(false, (sender, e) =>
        {
            SelectedToggleButton control = (SelectedToggleButton)sender;

            if (e.NewValue is bool value)
            {
                control._vsIsChecked = value;
                control.VisualState = control.VisualState;//State         
            }
        }));


        #endregion

        
        //@VisualState
        ClickMode _vsClickMode;
        bool _vsIsChecked = false;
        bool _vsIsEnabled = true;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsChecked == false)
                {
                    if (this._vsIsEnabled == false) return this.Disable;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Normal;
                        case ClickMode.Hover: return this.PointerOver;
                        case ClickMode.Press: return this.Pressed;
                    }
                }

                if (this._vsIsChecked)
                {
                    if (this._vsIsEnabled == false) return this.CheckedDisable;

                    switch (this._vsClickMode)
                    {
                        case ClickMode.Release: return this.Checked;
                        case ClickMode.Hover: return this.CheckedPointerOver;
                        case ClickMode.Press: return this.CheckedPressed;
                    }
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }
        /// <summary> VisualState's ClickMode. </summary>
        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a SelectedToggleButton. 
        /// </summary>
        public SelectedToggleButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State 
            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = this.IsEnabled;
                this.VisualState = this.VisualState;//State 
            };

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}