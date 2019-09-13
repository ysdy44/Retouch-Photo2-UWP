namespace Retouch_Photo2.Operates
{
    /// <summary>
    /// Manager of <see cref="OperateButton"/>. 
    /// </summary>
    public class OperateButtonStateManager
    {
        /// <summary> 
        /// PointerState of <see cref="OperateButtonStateManager"/>. 
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

        public OperateButtonState GetState()
        {
            if (this.IsEnabled == false) return OperateButtonState.Disabled;

            switch (this.PointerState)
            {
                case ButtonPointerState.None: return OperateButtonState.None;
                case ButtonPointerState.Pressed: return OperateButtonState.Pressed;
                case ButtonPointerState.PointerOver: return OperateButtonState.PointerOver;
            }

            return OperateButtonState.None;
        }
    }

}