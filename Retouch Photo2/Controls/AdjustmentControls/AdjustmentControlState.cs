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

        /// <summary> There is no adjustment. </summary>
        ZeroAdjustments,
        /// <summary> There are some adjustments.. </summary>
        Adjustments, 

        /// <summary> Editing adjustments. </summary>
        Edit,
        /// <summary> Filters list. </summary>
        Filters,
    }
}