using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class HueRotationAdjustmentItem : AdjustmentItem
    {
        public float Angle;

        public HueRotationAdjustmentItem() => base.Name = HueRotationAdjustment.Name;

        public override Adjustment GetAdjustment() => new HueRotationAdjustment()
        {
            HueRotationAdjustmentitem = this
        };
    }
}
