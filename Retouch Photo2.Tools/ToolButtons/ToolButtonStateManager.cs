namespace Retouch_Photo2.Tools
{
    /// <summary>
    /// Manager of <see cref="ToolButton"/>. 
    /// </summary>
    public class ToolButtonStateManager
    {
        /// <summary> 
        /// PointerState of <see cref="ToolButtonStateManager"/>. 
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

        public ToolButtonState GetState()
        {
            switch (this.IsSelected)
            {
                case null: return ToolButtonState.None;

                case false:
                    {
                        switch (this.PointerState)
                        {
                            case ButtonPointerState.None: return ToolButtonState.None;
                            case ButtonPointerState.Pressed: return ToolButtonState.Pressed;
                            case ButtonPointerState.PointerOver: return ToolButtonState.PointerOver;
                        }
                    }
                    break;

                case true:
                    {
                        switch (this.PointerState)
                        {
                            case ButtonPointerState.None: return ToolButtonState.Selected;
                            case ButtonPointerState.Pressed: return ToolButtonState.PressedSelected;
                            case ButtonPointerState.PointerOver: return ToolButtonState.PointerOverSelected;
                        }
                    }
                    break;

                default:
                    break;
            }

            return ToolButtonState.None;
        }
    }

}