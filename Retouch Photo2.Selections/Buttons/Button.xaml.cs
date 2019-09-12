using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Selections.Buttons
{
    /// <summary>
    /// Retouch_Photo2 Selections 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        //@Content
        /// <summary> Enabled icon. </summary>
        public UIElement EnabledIcon { get => this.EnabledViewbox.Child; set => this.EnabledViewbox.Child = value; }
        /// <summary> Disabled icon. </summary>
        public UIElement DisabledIcon { get => this.DisabledViewbox.Child; set => this.DisabledViewbox.Child = value; }
        /// <summary> TextBlock' text. </summary>
        public string Label { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }

        
        /// <summary> Manager of <see cref="Button"/>. </summary>
        ButtonStateManager Manager = new ButtonStateManager();
        /// <summary> State of <see cref="Button"/>. </summary>
        public ButtonState State
        {
            set
            {
                switch (value)
                {
                    case ButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case ButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case ButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case ButtonState.Disabled: VisualStateManager.GoToState(this, this.Disabled.Name, false); break;
                }
            }
        }


        //@Construct
        public Button()
        {
            this.InitializeComponent();
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.None;
                this.State = this.Manager.GetState();//State
            };

            this.Loaded += (s, e) => this.SetIsEnabled(this.IsEnabled);
            this.IsEnabledChanged += (s, e) => this.SetIsEnabled((bool)e.NewValue);
        }

        void SetIsEnabled(bool isEnabled)
        {
            this.Manager.IsEnabled = isEnabled;
            this.State = this.Manager.GetState();//State
        }
    }
}