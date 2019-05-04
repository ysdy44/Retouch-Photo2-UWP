using Retouch_Photo2.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;
namespace Retouch_Photo2.Adjustments.Items
{
    public class GrayAdjustmentItem : AdjustmentItem
    {
        public GrayAdjustmentItem() => base.Name = GrayAdjustment.Name;

        //@override
        public override Adjustment GetNewAdjustment()
        {
            GrayAdjustment adjustment = new GrayAdjustment();

            return adjustment;
        }
        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new GrayscaleEffect
            {
                Source = image
            };
        }
    }
}
