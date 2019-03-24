using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;

namespace Retouch_Photo.Adjustments.Models
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
