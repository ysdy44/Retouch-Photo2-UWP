using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class ToolButton : UserControl
    {
        //@Content
        /// <summary> Button's IsSelected. </summary>
        public bool IsSelected
        {
            set
            {
                if (this.Manager.IsSelected == value) return;

                this.Manager.IsSelected = value;
                this.State = this.Manager.GetState();
            }
        }
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }

        
        /// <summary> Manager of <see cref="ToolButton"/>. </summary>
        ToolButtonStateManager Manager = new ToolButtonStateManager();
        /// <summary> State of <see cref="ToolButton"/>. </summary>
        public ToolButtonState State
        {
            set
            {
                switch (value)
                {
                    case ToolButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case ToolButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case ToolButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case ToolButtonState.Selected: VisualStateManager.GoToState(this, this.Selected.Name, false); break;
                    case ToolButtonState.PointerOverSelected: VisualStateManager.GoToState(this, this.PointerOverSelected.Name, false); break;
                    case ToolButtonState.PressedSelected: VisualStateManager.GoToState(this, this.PressedSelected.Name, false); break;
                }
            }
        }


        //@Construct
        public ToolButton()
        {
            this.InitializeComponent();
            this.ContentPresenter.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = ToolButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = ToolButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = ToolButtonStateManager.ButtonPointerState.None;
                this.State = this.Manager.GetState();//State
            };
        }
    }
}