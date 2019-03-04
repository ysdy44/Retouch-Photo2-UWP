using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
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


