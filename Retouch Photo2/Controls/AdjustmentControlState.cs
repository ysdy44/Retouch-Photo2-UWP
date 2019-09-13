namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// State of <see cref="AdjustmentControl"/>. 
    /// </summary>
    public enum AdjustmentControlState
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Control is not available. </summary>
        Disable,
        /// <summary> Control no adjustment. </summary>
        Null,
        /// <summary> There are adjustments.. </summary>
        Adjustments,
        /// <summary> Editing adjustments. </summary>
        Edit
    }
}