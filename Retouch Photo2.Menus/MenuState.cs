namespace Retouch_Photo2.Menus
{
    /// <summary> 
    /// State of <see cref="MenuBase"/>.
    /// </summary>
    public enum MenuState
    {
        /// <summary> Flyout hided. </summary>
        FlyoutHide,

        /// <summary> Flyout showed. </summary>
        FlyoutShow,

        /// <summary> Overlay expanded. </summary>
        OverlayExpanded,

        /// <summary> Overlay not expanded. </summary>
        OverlayNotExpanded
    }
}