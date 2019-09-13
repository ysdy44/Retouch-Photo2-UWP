using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Effects
{
    /// <summary>
    /// Manager of <see cref="EffectButton"/>. 
    /// </summary>
    public class EffectButtonStateManager
    {
        /// <summary> 
        /// PointerState of <see cref="EffectButtonStateManager"/>. 
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
        public bool IsOn;

        public ButtonPointerState PointerState;

        public EffectButtonState GetState()
        {
            if (this.IsEnabled == false) return EffectButtonState.Disabled;
            if (this.IsOn == false) return EffectButtonState.NonDisabled;

            switch (this.PointerState)
            {
                case ButtonPointerState.None: return EffectButtonState.None;
                case ButtonPointerState.Pressed: return EffectButtonState.Pressed;
                case ButtonPointerState.PointerOver: return EffectButtonState.PointerOver;
            }

            return EffectButtonState.None;
        }
    }
}