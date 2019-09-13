namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// State of <see cref="MenuButton"/>.
    /// </summary>
    public enum MenuButtonState
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