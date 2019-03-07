using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class ExposureAdjustmentItem : AdjustmentItem
    {
        public float Exposure;

        public ExposureAdjustmentItem() => base.Type = AdjustmentType.Exposure;

        public override Adjustment GetAdjustment() => new ExposureAdjustment()
        {
            ExposureAdjustmentItem = this
        };
    }
}
