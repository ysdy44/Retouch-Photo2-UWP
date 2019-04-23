using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    public class GrayAdjustment: Adjustment
    {
        public static readonly string Name = "Gray";
        public GrayAdjustmentItem GrayAdjustmentItem = new GrayAdjustmentItem();

        public GrayAdjustment()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new GrayControl();
            base.Item = this.GrayAdjustmentItem;
            base.Item.Reset();
            base.HasPage = false;
        }
    }
}
