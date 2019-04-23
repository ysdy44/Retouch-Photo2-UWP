using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;

namespace Retouch_Photo2.Adjustments.Models
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


