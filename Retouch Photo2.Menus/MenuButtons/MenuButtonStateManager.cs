namespace Retouch_Photo2.Menus
{
    /// <summary>
    /// Manager of <see cref="MenuButton"/>. 
    /// </summary>
    public class MenuButtonStateManager 
    {
        /// <summary> 
        /// PointerState of <see cref="MenuButtonStateManager"/>. 
        /// </summary>
        public enum ButtonPointerState
        {
            /// <summary> Normal. </summary>
            None,
            /// <summary> Pointer-over. </summary>
            PointerOver,
            /// <summary> Pressed. </summary>
            Pressed,
        }

        public MenuState MenuState;
        public ButtonPointerState PointerState;

        public MenuButtonState GetState()
        {
            if (this.MenuState== MenuState.FlyoutShow) return MenuButtonState.Flyout;

            if (this.MenuState == MenuState.FlyoutHide)
            {
                switch (this.PointerState)
                {
                    case ButtonPointerState.None: return MenuButtonState.None;
                    case ButtonPointerState.PointerOver: return MenuButtonState.PointerOver;
                    case ButtonPointerState.Pressed: return MenuButtonState.Pressed;
                }
            }

            if (this.MenuState == MenuState.OverlayExpanded || this.MenuState == MenuState.OverlayNotExpanded)
            {
                switch (this.PointerState)
                {
                    case ButtonPointerState.None: return MenuButtonState.Overlay;
                    case ButtonPointerState.PointerOver: return MenuButtonState.PointerOverOverlay;
                    case ButtonPointerState.Pressed: return MenuButtonState.PressedOverlay;
                }
            }

            return MenuButtonState.None;
        }
    }
}