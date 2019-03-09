using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class HighlightsAndShadowsAdjustment : Adjustment
    {
        public static readonly string Name = "HighlightsAndShadows";
        public HighlightsAndShadowsAdjustmentItem HighlightsAndShadowsAdjustmentItem = new HighlightsAndShadowsAdjustmentItem();

        public HighlightsAndShadowsAdjustment()
        {
            base.Type = AdjustmentType.HighlightsAndShadows;
            base.Icon = new HighlightsAndShadowsControl();
            base.Item = this.HighlightsAndShadowsAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.HighlightsAndShadowsAdjustmentItem.Shadows = 0.0f;
            this.HighlightsAndShadowsAdjustmentItem.Highlights = 0.0f;
            this.HighlightsAndShadowsAdjustmentItem.Clarity = 0.0f;
            this.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount = 1.25f;
            this.HighlightsAndShadowsAdjustmentItem.SourceIsLinearGamma = false;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new HighlightsAndShadowsEffect
            {
                Shadows = this.HighlightsAndShadowsAdjustmentItem.Shadows,
                Highlights = this.HighlightsAndShadowsAdjustmentItem.Highlights,
                Clarity = this.HighlightsAndShadowsAdjustmentItem.Clarity,
                MaskBlurAmount = this.HighlightsAndShadowsAdjustmentItem.MaskBlurAmount,
                SourceIsLinearGamma = this.HighlightsAndShadowsAdjustmentItem.SourceIsLinearGamma,
                Source = image
            };
        }
    }
}


