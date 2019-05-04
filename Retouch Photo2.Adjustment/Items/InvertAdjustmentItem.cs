using Retouch_Photo2.Adjustments.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;
namespace Retouch_Photo2.Adjustments.Items
{
    public class InvertAdjustmentItem : AdjustmentItem
    {
        public InvertAdjustmentItem() => base.Name = InvertAdjustment.Name;

        //@override
        public override Adjustment GetNewAdjustment()
        {
            InvertAdjustment adjustment = new InvertAdjustment();

            return adjustment;
        }
        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect
            {
                Source = image
            };
        }
    }
}
