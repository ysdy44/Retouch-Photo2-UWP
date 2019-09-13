namespace Retouch_Photo2.Selections
{
    /// <summary>
    /// Manager of <see cref="SelectionButton"/>. 
    /// </summary>
    public class SelectionButtonStateManager
    {
        /// <summary> 
        /// PointerState of <see cref="SelectionButtonStateManager"/>. 
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

        public SelectionButtonState GetState()
        {
            if (this.IsEnabled == false) return SelectionButtonState.Disabled;

            switch (this.PointerState)
            {
                case ButtonPointerState.None: return SelectionButtonState.None;
                case ButtonPointerState.Pressed: return SelectionButtonState.Pressed;
                case ButtonPointerState.PointerOver: return SelectionButtonState.PointerOver;
            }

            return SelectionButtonState.None;
        }
    }

}