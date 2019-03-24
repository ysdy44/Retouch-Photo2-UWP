using Retouch_Photo.Adjustments.Models;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Adjustments.Items
{
    public class VignetteAdjustmentItem : AdjustmentItem
    {
        public float Amount;
        public float Curve;
        public Color Color;

        public VignetteAdjustmentItem() => base.Name = VignetteAdjustment.Name;

        //@override
        public override Adjustment GetAdjustment() => new VignetteAdjustment()
        {
            VignetteAdjustmentItem = this
        };
        public override void Reset()
        {
            this.Amount = 0.0f;
            this.Curve = 0.0f;
            this.Color = Colors.Black;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new VignetteEffect
            {
                Amount = this.Amount,
                Curve = this.Curve,
                Color = this.Color,
                Source = image
            };
        }
    }
}
