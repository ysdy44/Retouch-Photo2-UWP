using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus.MenuButtons
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class Button : UserControl
    {
        //@Content 
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }


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

                    case ButtonState.Flyout: VisualStateManager.GoToState(this, this.Flyout.Name, false); break;

                    case ButtonState.Overlay: VisualStateManager.GoToState(this, this.Overlay.Name, false); break;
                    case ButtonState.PointerOverOverlay: VisualStateManager.GoToState(this, this.PointerOverOverlay.Name, false); break;
                    case ButtonState.PressedOverlay: VisualStateManager.GoToState(this, this.PressedOverlay.Name, false); break;
                }
            }
        }


        //@Construct
        public Button()
        {
            this.InitializeComponent();
            this.ContentPresenter.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.ContentPresenter.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = ButtonStateManager.ButtonPointerState.None;
                this.State = this.Manager.GetState();//State
            };
        }

        
        public void SetMenuState(MenuState state)
        {
            this.Manager.MenuState = state;
            this.State = this.Manager.GetState();//State
        }
    }
}