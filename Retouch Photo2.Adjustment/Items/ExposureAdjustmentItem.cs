using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="ExposureAdjustment">.
    /// </summary>
    public class ExposureAdjustmentItem : AdjustmentItem
    {
        public float Exposure;

        //@Construct
        public ExposureAdjustmentItem()
        {
            base.Name = ExposureAdjustment.Name;
        }

        //@Override
        public override Adjustment GetNewAdjustment()
        {
            ExposureAdjustment adjustment = new ExposureAdjustment();

            adjustment.ExposureAdjustmentItem.Exposure = this.Exposure;

            return adjustment;
        }
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
