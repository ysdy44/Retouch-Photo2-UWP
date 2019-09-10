namespace Retouch_Photo2.Menus.MenuButtons
{
    /// <summary>
    /// Manager of <see cref="Button"/>. 
    /// </summary>
    public class ButtonStateManager
    {
        /// <summary> 
        /// PointerState of <see cref="ButtonStateManager"/>. 
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

        public ButtonState GetState()
        {
            if (this.MenuState== MenuState.FlyoutShow) return ButtonState.Flyout;

            if (this.MenuState == MenuState.FlyoutHide)
            {
                switch (this.PointerState)
                {
                    case ButtonPointerState.None: return ButtonState.None;
                    case ButtonPointerState.PointerOver: return ButtonState.PointerOver;
                    case ButtonPointerState.Pressed: return ButtonState.Pressed;
                }
            }

            if (this.MenuState == MenuState.OverlayExpanded || this.MenuState == MenuState.OverlayNotExpanded)
            {
                switch (this.PointerState)
                {
                    case ButtonPointerState.None: return ButtonState.Overlay;
                    case ButtonPointerState.PointerOver: return ButtonState.PointerOverOverlay;
                    case ButtonPointerState.Pressed: return ButtonState.PressedOverlay;
                }
            }

            return ButtonState.None;
        }
    }
}