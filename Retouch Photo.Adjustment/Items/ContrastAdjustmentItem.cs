using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class ContrastAdjustmentItem: AdjustmentItem
    {
        public float Contrast;

        public ContrastAdjustmentItem() => base.Name = ContrastAdjustment.Name;
 
        public override Adjustment GetAdjustment() => new ContrastAdjustment()
        {
            ContrastAdjustmentItem = this
        };
    }
}
