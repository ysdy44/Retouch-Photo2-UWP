using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s VignetteAdjustment.
    /// </summary>
    public class VignetteAdjustment : Adjustment
    {
        public const string Name = "Vignette";
        public VignetteAdjustmentItem VignetteAdjustmentItem = new VignetteAdjustmentItem();

        //@Construct
        public VignetteAdjustment()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.Item = this.VignetteAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}