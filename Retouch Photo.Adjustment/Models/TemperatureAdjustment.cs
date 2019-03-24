using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;

namespace Retouch_Photo.Adjustments.Models
{
    public class TemperatureAdjustment : Adjustment
    {
        public static readonly string Name = "Temperature";
        public TemperatureAdjustmentItem TemperatureAdjustmentItem =new TemperatureAdjustmentItem();

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

