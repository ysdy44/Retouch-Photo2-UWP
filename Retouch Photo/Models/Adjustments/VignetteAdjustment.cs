using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentsControls;
using Retouch_Photo.Pages.AdjustmentPages;
using Windows.UI;

namespace Retouch_Photo.Models.Adjustments
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


    public class VignetteAdjustmentCandidate : AdjustmentCandidate
    {
        public VignettePage page = new VignettePage();

        public VignetteAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new VignetteAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is VignetteAdjustment VignetteAdjustment)
            {
                this.page.VignetteAdjustment = null;
                this.page.VignetteAdjustment = VignetteAdjustment;
            }
        }
    }
}

