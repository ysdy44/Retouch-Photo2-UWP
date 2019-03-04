using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;
using Windows.UI;

namespace Retouch_Photo.Adjustments.Models
{
    public class VignetteAdjustment : Adjustment
    {
        public float Amount;
        public float Curve;
        public Color Color = Colors.Black;

        public VignetteAdjustment()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.HasPage = true;
            this.Reset();
        }

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

