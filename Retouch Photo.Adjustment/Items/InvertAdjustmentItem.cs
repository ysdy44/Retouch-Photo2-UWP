using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class InvertAdjustmentItem : AdjustmentItem
    {
        public InvertAdjustmentItem() => base.Type = AdjustmentType.Invert;

        public override Adjustment GetAdjustment() => new InvertAdjustment()
        {
            InvertAdjustmentItem = this
        };
    }
}
