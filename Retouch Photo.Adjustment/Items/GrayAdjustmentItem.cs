using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class GrayAdjustmentItem : AdjustmentItem
    {
        public GrayAdjustmentItem() => base.Type = AdjustmentType.Gray;

        public override Adjustment GetAdjustment() => new GrayAdjustment()
        {
            GrayAdjustmentItem = this
        };
    }
}
