// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      
namespace Retouch_Photo2.Edits
{
    /// <summary> 
    /// Type of Edit.
    /// </summary>
    public enum EditType
    {
        /// <summary> Normal </summary>
        None,

        //Edit
        /// <summary> Cut </summary>
        Edit_Cut,
        /// <summary> Duplicate </summary>
        Edit_Duplicate,
        /// <summary> Copy </summary>
        Edit_Copy,
        /// <summary> Paste </summary>
        Edit_Paste,
        /// <summary> Clear </summary>
        Edit_Clear,

        //Select
        /// <summary> All </summary>
        Select_All,
        /// <summary> Deselect </summary>
        Select_Deselect,
        /// <summary> Invert </summary>
        Select_Invert,

        //Group
        /// <summary> Group </summary>
        Group_Group,
        /// <summary> UnGroup </summary>
        Group_UnGroup,
        /// <summary> Release </summary>
        Group_Release,

        //Combine_
        /// <summary> Union </summary>
        Combine_Union,
        /// <summary> Exclude </summary>
        Combine_Exclude,
        /// <summary> Xor </summary>
        Combine_Xor,
        /// <summary> Intersect </summary>
        Combine_Intersect,
        /// <summary> Expand Stroke </summary>
        Combine_ExpandStroke,
    }
}