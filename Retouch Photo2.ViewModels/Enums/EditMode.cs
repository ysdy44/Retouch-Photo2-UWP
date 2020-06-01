namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Mode of the <see cref = "SettingViewModel.Edit" />. 
    /// </summary>
    public enum EditMode
    {
        /// <summary> Normal </summary>
        None,

        //Edit
        /// <summary> Normal </summary>
        Cut,
        /// <summary> Normal </summary>
        Duplicate,
        /// <summary> Normal </summary>
        Copy,
        /// <summary> Normal </summary>
        Paste,
        /// <summary> Normal </summary>
        Clear,

        //Select
        /// <summary> All </summary>
        All,
        /// <summary> Deselect </summary>
        Deselect,
        /// <summary> Invert </summary>
        Invert,

        //Group
        /// <summary> Group </summary>
        Group,
        /// <summary> UnGroup </summary>
        UnGroup,
        /// <summary> Release </summary>
        Release,

        //Combine
        /// <summary> Union </summary>
        Union,
        /// <summary> Exclude </summary>
        Exclude,
        /// <summary> Xor </summary>
        Xor,
        /// <summary> Intersect </summary>
        Intersect,
        /// <summary> Expand Stroke </summary>
        ExpandStroke,

        //Undo
        /// <summary> Undo </summary>
        Undo,
        /// <summary> Redo </summary>
        Redo,
    }
}