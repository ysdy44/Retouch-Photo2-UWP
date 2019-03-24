using Retouch_Photo.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Models;
namespace Retouch_Photo.Adjustments.Items
{
    public class HueRotationAdjustmentItem : AdjustmentItem
    {
        public float Angle;

        public HueRotationAdjustmentItem() => base.Name = HueRotationAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new HueRotationAdjustment()
        {
            HueRotationAdjustmentitem = this
        };
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
