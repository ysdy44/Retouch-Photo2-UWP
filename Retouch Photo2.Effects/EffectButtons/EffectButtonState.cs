namespace Retouch_Photo2.Effects
{
    /// <summary> 
    /// State of <see cref="EffectButton"/>.
    /// </summary>
    public enum EffectButtonState
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Pointer-over. </summary>
        PointerOver,
        /// <summary> Pressed. </summary>
        Pressed,

        /// <summary> Disabled. </summary>
        Disabled,
        /// <summary> Non-disabled. </summary>
        NonDisabled,        
    }
}