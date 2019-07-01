using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="GrayAdjustment">.
    /// </summary>
    public class GrayAdjustmentItem : AdjustmentItem
    {
        //@Construct
        public GrayAdjustmentItem()
        {
            base.Name = GrayAdjustment.Name;
        }

        //@Override
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
