namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// State of <see cref="Expander"/>.
    /// </summary>
    public enum ExpanderState
    {
        /// <summary> Hided. </summary>
        Hide,

        /// <summary> Flyout showed. </summary>
        FlyoutShow,

        /// <summary> Overlay not expanded. </summary>
        OverlayNotExpanded,

        /// <summary> Overlay expanded. </summary>
        Overlay,
    }
}