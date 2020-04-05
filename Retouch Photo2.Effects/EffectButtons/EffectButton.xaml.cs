using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    public sealed partial class EffectButton : UserControl
    {
        //@Content 
        /// <summary> Viewbox's icon. </summary>
        public UIElement Icon { get => this.Viewbox.Child; set => this.Viewbox.Child = value; }
        /// <summary> TextBlock' text. </summary>
        public string Label { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }
        /// <summary> EffectButton' ToggleSwitch. </summary>
        public ToggleSwitch ToggleSwitch => this._ToggleSwitch;


        //@VisualState
        bool _vsIsEnabled => this._ToggleSwitch.IsEnabled;
        bool _vsIsOn => this._ToggleSwitch.IsOn;
        ClickMode _vsClickMode;
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;
                if (this._vsIsOn == false) return this.NonDisabled;

                switch (this._vsClickMode)
                {
                    case ClickMode.Release: return this.Normal;
                    case ClickMode.Hover: return this.PointerOver;
                    case ClickMode.Press: return this.Pressed;
                }
                return this.Normal;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        private ClickMode ClickMode
        {
            set
            {
                this._vsClickMode = value;
                this.VisualState = this.VisualState;//State
            }
        }

        //@Construct
        public EffectButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this._ToggleSwitch.IsEnabledChanged += (s, e) => this.VisualState = this.VisualState;//State
            this._ToggleSwitch.Toggled += (s, e) => this.VisualState = this.VisualState;//State

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;
        }
    }
}