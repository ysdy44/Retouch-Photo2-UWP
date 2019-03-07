using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class TemperatureAdjustmentItem: AdjustmentItem
    {
        public float Temperature;
        public float Tint;

        public TemperatureAdjustmentItem() => base.Type = AdjustmentType.Temperature;

        public override Adjustment GetAdjustment() => new TemperatureAdjustment()
        {
            TemperatureAdjustmentItem = this
        };
    }
}
