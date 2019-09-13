using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Selections
{
    /// <summary>
    /// Retouch_Photo2 Selections 's Button.
    /// </summary>
    public sealed partial class SelectionButton : UserControl
    {
        //@Content
        /// <summary> Enabled icon. </summary>
        public UIElement EnabledIcon { get => this.EnabledViewbox.Child; set => this.EnabledViewbox.Child = value; }
        /// <summary> Disabled icon. </summary>
        public UIElement DisabledIcon { get => this.DisabledViewbox.Child; set => this.DisabledViewbox.Child = value; }
        /// <summary> TextBlock' text. </summary>
        public string Label { get => this.TextBlock.Text; set => this.TextBlock.Text = value; }

        
        /// <summary> Manager of <see cref="SelectionButton"/>. </summary>
        SelectionButtonStateManager Manager = new SelectionButtonStateManager();
        /// <summary> State of <see cref="SelectionButton"/>. </summary>
        public SelectionButtonState State
        {
            set
            {
                switch (value)
                {
                    case SelectionButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case SelectionButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case SelectionButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case SelectionButtonState.Disabled: VisualStateManager.GoToState(this, this.Disabled.Name, false); break;
                }
            }
        }


        //@Construct
        public SelectionButton()
        {
            this.InitializeComponent();
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = SelectionButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = SelectionButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = SelectionButtonStateManager.ButtonPointerState.None;
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