namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// background mode of <see cref="ILayer"/>.
    /// </summary>
    public enum BackgroundMode
    {
        /// <summary> Not selected. </summary>
        UnSelected,
        /// <summary> Selected. </summary>
        Selected,

        /// <summary> Parents were selected. </summary>
        ParentsSelected,
        /// <summary> Child is selected. </summary>
        ChildSelected,

    }
}