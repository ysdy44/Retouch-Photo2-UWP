using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Models;

namespace Retouch_Photo2.Adjustments.Items
{
    /// <summary>
    /// Item of <see cref="ContrastAdjustment">.
    /// </summary>
    public class ContrastAdjustmentItem: AdjustmentItem
    {
        public float Contrast;

        //@Construct
        public ContrastAdjustmentItem()
        {
            base.Name = ContrastAdjustment.Name;
        }

        //@Override
        public override Adjustment GetNewAdjustment()
        {
            ContrastAdjustment adjustment = new ContrastAdjustment();

            adjustment.ContrastAdjustmentItem.Contrast = this.Contrast;

            return adjustment;
        }
        public override void Reset()
        {
            this.Contrast = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ContrastEffect
            {
                Contrast = this.Contrast,
                Source = image
            };
        }
    }
}
