namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// State of <see cref="Button"/>.
    /// </summary>
    public enum ButtonState
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Pointer-over. </summary>
        PointerOver,
        /// <summary> Pressed. </summary>
        Pressed,

        /// <summary> Selected. </summary>
        Selected,
        /// <summary> Selected (Pointer-over). </summary>
        PointerOverSelected,
        /// <summary> Selected (Pressed). </summary>
        PressedSelected,
    }
}