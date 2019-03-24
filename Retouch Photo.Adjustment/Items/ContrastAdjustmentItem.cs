using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class ContrastAdjustmentItem: AdjustmentItem
    {
        public float Contrast;

        public ContrastAdjustmentItem() => base.Name = ContrastAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new ContrastAdjustment()
        {
            ContrastAdjustmentItem = this
        };
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
