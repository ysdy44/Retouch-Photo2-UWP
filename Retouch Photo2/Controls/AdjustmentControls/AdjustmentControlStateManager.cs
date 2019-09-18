using Retouch_Photo2.Adjustments;
using System.Collections.Generic;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Manager of <see cref="AdjustmentControlState"/>. 
    /// </summary>
    public class AdjustmentControlStateManager
    {
        public bool IsEdit;
        public bool IsFilter;

        public List<IAdjustment> Adjustments;

        /// <summary>
        /// Return status based on propertys.
        /// </summary>
        public AdjustmentControlState GetState()
        {
            if (this.IsEdit) return AdjustmentControlState.Edit;
            if (this.IsFilter) return AdjustmentControlState.Filters;
            
            if (this.Adjustments==null) return AdjustmentControlState.Disable;
            
            if (this.Adjustments.Count == 0)
            {
                return AdjustmentControlState.ZeroAdjustments;
            }
            else
            {
                return AdjustmentControlState.Adjustments;
            }
        }
    }
}