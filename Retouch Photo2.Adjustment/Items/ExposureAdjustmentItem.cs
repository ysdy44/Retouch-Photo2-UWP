using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    public class ExposureAdjustmentItem : AdjustmentItem
    {
        public float Exposure;

        public ExposureAdjustmentItem() => base.Name = ExposureAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new ExposureAdjustment()
        {
            ExposureAdjustmentItem = this
        };
        public override void Reset()
        {
            this.Exposure = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure = this.Exposure,
                Source = image
            };
        }
    }
}
