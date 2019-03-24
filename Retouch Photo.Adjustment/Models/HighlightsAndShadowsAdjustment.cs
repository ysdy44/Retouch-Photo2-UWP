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
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}


