using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s TemperatureAdjustment.
    /// </summary>
    public class TemperatureAdjustment : Adjustment
    {
        public const string Name = "Temperature";
        public TemperatureAdjustmentItem TemperatureAdjustmentItem =new TemperatureAdjustmentItem();

        //@Construct
        public TemperatureAdjustment()
        {
            base.Type = AdjustmentType.Temperature;
            base.Icon = new TemperatureControl();
            base.Item = this.TemperatureAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}