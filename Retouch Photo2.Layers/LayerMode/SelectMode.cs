namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// Select mode of <see cref="ILayer"/>.
    /// </summary>
    public enum SelectMode
    {
        /// <summary> Normal. </summary>
        None,

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