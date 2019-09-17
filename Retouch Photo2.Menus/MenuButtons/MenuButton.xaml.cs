using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Retouch_Photo2 Tools 's Button.
    /// </summary>
    public sealed partial class MenuButton : UserControl
    {
        //@Content 
        /// <summary> ContentPresenter's Content. </summary>
        public object CenterContent { set => this.ContentPresenter.Content = value; get => this.ContentPresenter.Content; }


        /// <summary> Manager of <see cref="MenuButton"/>. </summary>
        MenuButtonStateManager Manager = new MenuButtonStateManager();
        /// <summary> State of <see cref="MenuButton"/>. </summary>
        public MenuButtonState State
        {
            set
            {
                switch (value)
                {
                    case MenuButtonState.None: VisualStateManager.GoToState(this, this.Normal.Name, false); break;
                    case MenuButtonState.PointerOver: VisualStateManager.GoToState(this, this.PointerOver.Name, false); break;
                    case MenuButtonState.Pressed: VisualStateManager.GoToState(this, this.Pressed.Name, false); break;

                    case MenuButtonState.Flyout: VisualStateManager.GoToState(this, this.Flyout.Name, false); break;

                    case MenuButtonState.Overlay: VisualStateManager.GoToState(this, this.Overlay.Name, false); break;
                    case MenuButtonState.PointerOverOverlay: VisualStateManager.GoToState(this, this.PointerOverOverlay.Name, false); break;
                    case MenuButtonState.PressedOverlay: VisualStateManager.GoToState(this, this.PressedOverlay.Name, false); break;
                }
            }
        }


        //@Construct
        public MenuButton()
        {
            this.InitializeComponent();
            this.RootGrid.PointerEntered += (s, e) =>
            {
                this.Manager.PointerState = MenuButtonStateManager.ButtonPointerState.PointerOver;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerPressed += (s, e) =>
            {
                this.Manager.PointerState = MenuButtonStateManager.ButtonPointerState.Pressed;
                this.State = this.Manager.GetState();//State
            };
            this.RootGrid.PointerExited += (s, e) =>
            {
                this.Manager.PointerState = MenuButtonStateManager.ButtonPointerState.None;
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