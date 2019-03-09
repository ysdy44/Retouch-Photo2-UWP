using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class InvertAdjustmentItem : AdjustmentItem
    {
        public InvertAdjustmentItem() => base.Name = InvertAdjustment.Name;

        public override Adjustment GetAdjustment() => new InvertAdjustment()
        {
            InvertAdjustmentItem = this
        };
    }
}
