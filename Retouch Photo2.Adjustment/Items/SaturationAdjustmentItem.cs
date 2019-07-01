using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="SaturationAdjustment">.
    /// </summary>
    public class SaturationAdjustmentItem : AdjustmentItem
    {
        public float Saturation;

        //@Construct
        public SaturationAdjustmentItem()
        {
            base.Name = SaturationAdjustment.Name;
        }

        //@Override
        public override Adjustment GetNewAdjustment()
        {
            SaturationAdjustment adjustment = new SaturationAdjustment();

            adjustment.SaturationAdjustmentItem.Saturation = this.Saturation;

            return adjustment;
        }
        public override void Reset()
        {
            this.Saturation = 1.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }
    }
}
