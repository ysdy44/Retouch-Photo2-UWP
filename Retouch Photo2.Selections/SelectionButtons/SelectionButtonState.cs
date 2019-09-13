namespace Retouch_Photo2.Selections
{
    /// <summary> 
    /// State of <see cref="SelectionButton"/>.
    /// </summary>
    public enum SelectionButtonState
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