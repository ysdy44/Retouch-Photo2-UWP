namespace Retouch_Photo2.Operates.Buttons
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

        public bool IsEnabled;
        public ButtonPointerState PointerState;

        public ButtonState GetState()
        {
            if (this.IsEnabled == false) return ButtonState.Disabled;

            switch (this.PointerState)
            {
                case ButtonPointerState.None: return ButtonState.None;
                case ButtonPointerState.Pressed: return ButtonState.Pressed;
                case ButtonPointerState.PointerOver: return ButtonState.PointerOver;
            }

            return ButtonState.None;
        }
    }

}