using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="HueRotationAdjustment">.
    /// </summary>
    public class HueRotationAdjustmentItem : AdjustmentItem
    {
        public float Angle;

        //@Construct
        public HueRotationAdjustmentItem()
        {
            base.Name = HueRotationAdjustment.Name;
        }

        //@Override
        public override Adjustment GetNewAdjustment()
        {
            HueRotationAdjustment adjustment = new HueRotationAdjustment();

            adjustment.HueRotationAdjustmentitem.Angle = this.Angle;

            return adjustment;
        }
        public override void Reset()
        {
            this.Angle = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new HueRotationEffect
            {
                Angle = this.Angle,
                Source = image
            };
        }
    }
}
