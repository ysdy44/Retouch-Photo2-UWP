using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentsControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class HighlightsAndShadowsAdjustment : Adjustment
    {
        public float Shadows;
        public float Highlights;
        public float Clarity;
        public float MaskBlurAmount;
        public bool SourceIsLinearGamma;

        public HighlightsAndShadowsAdjustment()
        {
            base.Type = AdjustmentType.HighlightsAndShadows;
            base.Icon = new HighlightsAndShadowsControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.Shadows = 0.0f;
            this.Highlights = 0.0f;
            this.Clarity = 0.0f;
            this.MaskBlurAmount = 1.25f;
            this.SourceIsLinearGamma = false;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new HighlightsAndShadowsEffect
            {
                Shadows = this.Shadows,
                Highlights = this.Highlights,
                Clarity = this.Clarity,
                MaskBlurAmount = this.MaskBlurAmount,
                SourceIsLinearGamma = this.SourceIsLinearGamma,
                Source = image
            };
        }
    }


    public class HighlightsAndShadowsAdjustmentCandidate : AdjustmentCandidate
    {
        public HighlightsAndShadowsPage page = new HighlightsAndShadowsPage();

        public HighlightsAndShadowsAdjustmentCandidate()
        {
            base.Type = AdjustmentType.HighlightsAndShadows;
            base.Icon = new HighlightsAndShadowsControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new HighlightsAndShadowsAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is HighlightsAndShadowsAdjustment HighlightsAndShadowsAdjustment)
            {
                this.page.HighlightsAndShadowsAdjustment = null;
                this.page.HighlightsAndShadowsAdjustment = HighlightsAndShadowsAdjustment;
            }
        }
    }
}


