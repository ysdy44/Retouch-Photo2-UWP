using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s HighlightsAndShadowsAdjustment.
    /// </summary>
    public class HighlightsAndShadowsAdjustment : Adjustment
    {
        public const string Name = "HighlightsAndShadows";
        public HighlightsAndShadowsAdjustmentItem HighlightsAndShadowsAdjustmentItem = new HighlightsAndShadowsAdjustmentItem();

        //@Construct
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