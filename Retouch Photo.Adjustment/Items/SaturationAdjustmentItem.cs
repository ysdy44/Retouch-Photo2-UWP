using Retouch_Photo.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class SaturationAdjustmentItem : AdjustmentItem
    {
        public float Saturation;

        public SaturationAdjustmentItem() => base.Name = SaturationAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new SaturationAdjustment()
        {
            SaturationAdjustmentItem = this
        };
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
