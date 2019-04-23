using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    public class InvertAdjustment : Adjustment
    {
        public static readonly string Name = "Invert";
        public InvertAdjustmentItem InvertAdjustmentItem = new InvertAdjustmentItem();

        public InvertAdjustment()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            base.Item = this.InvertAdjustmentItem;
            base.Item.Reset();
            base.HasPage = false;
        }
    }
}
