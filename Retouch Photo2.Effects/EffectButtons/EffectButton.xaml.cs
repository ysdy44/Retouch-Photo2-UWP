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

        /// <summary> Manager of <see cref="EffectButton"/>. </summary>
        EffectButtonStateManager Manager = new EffectButtonStateManager();
        /// <summary> State of <see cref="EffectButton"/>. </summary>
        EffectButtonState State
        {
            set
            {
                switch (value)
                {
                    case EffectButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case EffectButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case EffectButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case EffectButtonState.Disabled: VisualStateManager.GoToState(this, this.Disabled.Name, false); break;
                    case EffectButtonState.NonDisabled: VisualStateManager.GoToState(this, this.NonDisabled.Name, false); break;
                }
            }
        }


        //@Construct
        public EffectButton()
        {
            this.InitializeComponent();
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = EffectButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = EffectButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = EffectButtonStateManager.ButtonPointerState.None;
                this.State = this.Manager.GetState();//State
            };


            this.Loaded += (s, e) => this.State = this.Manager.GetState();//State
            this._ToggleSwitch.IsEnabledChanged += (s, e) =>
            {
                this.Manager.IsEnabled = this._ToggleSwitch.IsEnabled;
                this.State = this.Manager.GetState();//State
            };
            this._ToggleSwitch.Toggled += (s, e) =>
            {
                this.Manager.IsOn = this._ToggleSwitch.IsOn;
                this.State = this.Manager.GetState();//State
            };
        } 
    }
}