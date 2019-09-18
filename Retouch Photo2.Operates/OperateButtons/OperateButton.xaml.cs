using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Operates
{
    /// <summary>
    /// Retouch_Photo2 Operates 's Button.
    /// </summary>
    public sealed partial class OperateButton : UserControl
    {
        //@Content
        /// <summary> Enabled icon. </summary>
        public UIElement EnabledIcon { get => this.EnabledViewbox.Child; set => this.EnabledViewbox.Child = value; }
        /// <summary> Disabled icon. </summary>
        public UIElement DisabledIcon { get => this.DisabledViewbox.Child; set => this.DisabledViewbox.Child = value; }

        
        /// <summary> Manager of <see cref="OperateButton"/>. </summary>
        OperateButtonStateManager Manager = new OperateButtonStateManager();
        /// <summary> State of <see cref="OperateButton"/>. </summary>
        OperateButtonState State
        {
            set
            {
                switch (value)
                {
                    case OperateButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case OperateButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case OperateButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case OperateButtonState.Disabled: VisualStateManager.GoToState(this, this.Disabled.Name, false); break;
                }
            }
        }


        //@Construct
        public OperateButton()
        {
            this.InitializeComponent();
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = OperateButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = OperateButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = OperateButtonStateManager.ButtonPointerState.None;
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