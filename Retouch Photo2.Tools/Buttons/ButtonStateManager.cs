namespace Retouch_Photo2.Tools.Buttons
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

        public bool? IsSelected;
        public ButtonPointerState PointerState;

        public ButtonState GetState()
        {
            switch (this.IsSelected)
            {
                case null: return ButtonState.None;

                case false:
                    {
                        switch (this.PointerState)
                        {
                            case ButtonPointerState.None: return ButtonState.None;
                            case ButtonPointerState.Pressed: return ButtonState.Pressed;
                            case ButtonPointerState.PointerOver: return ButtonState.PointerOver;
                        }
                    }
                    break;

                case true:
                    {
                        switch (this.PointerState)
                        {
                            case ButtonPointerState.None: return ButtonState.Selected;
                            case ButtonPointerState.Pressed: return ButtonState.PressedSelected;
                            case ButtonPointerState.PointerOver: return ButtonState.PointerOverSelected;
                        }
                    }
                    break;

                default:
                    break;
            }

            return ButtonState.None;
        }
    }

}