namespace Retouch_Photo2.Menus.MenuButtons
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

        /// <summary> Flyout. </summary>
        Flyout,

        /// <summary> Overlay. </summary>
        Overlay,
        /// <summary> Overlay (Pointer-over). </summary>
        PointerOverOverlay,
        /// <summary> Overlay (Pressed). </summary>
        PressedOverlay,
    }
}