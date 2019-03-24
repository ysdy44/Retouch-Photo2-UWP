using Retouch_Photo.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Models;
namespace Retouch_Photo.Adjustments.Items
{
    public class GrayAdjustmentItem : AdjustmentItem
    {
        public GrayAdjustmentItem() => base.Name = GrayAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new GrayAdjustment()
        {
            GrayAdjustmentItem = this
        };
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
