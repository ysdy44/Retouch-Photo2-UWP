using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class SaturationAdjustmentItem : AdjustmentItem
    {
        public float Saturation;

        public SaturationAdjustmentItem() => base.Name = SaturationAdjustment.Name;

        public override Adjustment GetAdjustment() => new SaturationAdjustment()
        {
            SaturationAdjustmentItem = this
        };
    }
}
