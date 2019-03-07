using Retouch_Photo.Adjustments.Models;

namespace Retouch_Photo.Adjustments.Items
{
    public class HighlightsAndShadowsAdjustmentItem : AdjustmentItem
    {
        public float Shadows;
        public float Highlights;
        public float Clarity;
        public float MaskBlurAmount;
        public bool SourceIsLinearGamma;

        public HighlightsAndShadowsAdjustmentItem() => base.Type = AdjustmentType.HighlightsAndShadows;

        public override Adjustment GetAdjustment() => new HighlightsAndShadowsAdjustment()
        {
            HighlightsAndShadowsAdjustmentItem = this
        };
    }
}
