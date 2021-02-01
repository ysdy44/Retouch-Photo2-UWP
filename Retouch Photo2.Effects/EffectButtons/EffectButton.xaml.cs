// Core:              ★
// Referenced:   ★★
// Difficult:         ★
// Only:              ★★★★
// Complete:      ★★
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Button of <see cref="Effect"/>.
    /// </summary>
    public sealed partial class EffectButton : UserControl
    {
        //@Content 
        /// <summary> Viewbox's icon. </summary>
        public UIElement Icon { get => this.Viewbox.Child; set => this.Viewbox.Child = value; }
        /// <summary> TextBlock's text. </summary>
        public string Text { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }


        private bool _isButtonTapped = true;
        /// <summary> ToggleSwitch's Toggled. </summary>
        public Action<bool> Toggled;
        /// <summary> ToggleSwitch's IsOn. </summary>
        public bool IsOn
        {
            get => this.ToggleSwitch.IsOn;
            set
            {
                this._isButtonTapped = false;
                this.ToggleSwitch.IsOn = value;
                this._isButtonTapped = true;
            }
        }


        //@VisualState
        bool _vsIsEnabled = true;
        ClickMode _vsClickMode;
        /// <summary> 
        /// Represents the visual appearance of UI elements in a specific state.
        /// </summary>
        public VisualState VisualState
        {
            get
            {
                if (this._vsIsEnabled == false) return this.Disabled;

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
        /// Initializes a EffectButton. 
        /// </summary>
        public EffectButton()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.IsEnabledChanged += (s, e) =>
            {
                this._vsIsEnabled = (bool)e.NewValue;
                this.VisualState = this.VisualState;//State
            };

            this.PointerEntered += (s, e) => this.ClickMode = ClickMode.Hover;
            this.PointerPressed += (s, e) => this.ClickMode = ClickMode.Press;
            this.PointerReleased += (s, e) => this.ClickMode = ClickMode.Release;
            this.PointerExited += (s, e) => this.ClickMode = ClickMode.Release;


            this.ToggleSwitch.Toggled += (s, e) =>
            {
                if (this._isButtonTapped == false) return;
                bool isOn = this.ToggleSwitch.IsOn;

                this.Toggled?.Invoke(isOn);//Delegate
            };
        }
    }
}