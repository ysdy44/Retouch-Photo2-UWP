namespace Retouch_Photo2.Operates
{
    /// <summary> 
    /// State of <see cref="OperateButton"/>.
    /// </summary>
    public enum OperateButtonState
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Pointer-over. </summary>
        PointerOver,
        /// <summary> Pressed. </summary>
        Pressed,

        /// <summary> Disable. </summary>
        Disabled,
    }
}