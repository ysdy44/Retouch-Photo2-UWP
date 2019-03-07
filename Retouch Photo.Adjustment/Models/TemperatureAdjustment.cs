using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;

namespace Retouch_Photo.Adjustments.Models
{
    public class TemperatureAdjustment : Adjustment
    {
        public TemperatureAdjustmentItem TemperatureAdjustmentItem =new TemperatureAdjustmentItem();

        public TemperatureAdjustment()
        {
            base.Type = AdjustmentType.Temperature;
            base.Icon = new TemperatureControl();
            base.Item = this.TemperatureAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.TemperatureAdjustmentItem .Temperature = 0.0f;
            this.TemperatureAdjustmentItem .Tint = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new TemperatureAndTintEffect
            {
                Temperature = this.TemperatureAdjustmentItem .Temperature,
                Tint = this.TemperatureAdjustmentItem .Tint,
                Source = image
            };
        }
    }
}

