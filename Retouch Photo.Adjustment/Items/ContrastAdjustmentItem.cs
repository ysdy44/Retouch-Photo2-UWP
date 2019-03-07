using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class ContrastAdjustmentItem: AdjustmentItem
    {
        public float Contrast;

        public ContrastAdjustmentItem() => base.Type = AdjustmentType.Contrast;

        public override Adjustment GetAdjustment() => new ContrastAdjustment()
        {
            ContrastAdjustmentItem = this
        };
    }
}
