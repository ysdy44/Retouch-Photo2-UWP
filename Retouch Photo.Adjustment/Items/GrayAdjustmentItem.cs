using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class GrayAdjustmentItem : AdjustmentItem
    {
        public GrayAdjustmentItem() => base.Name = GrayAdjustment.Name;

        public override Adjustment GetAdjustment() => new GrayAdjustment()
        {
            GrayAdjustmentItem = this
        };
    }
}
