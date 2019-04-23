using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;

namespace Retouch_Photo2.Adjustments.Models
{
    public class ContrastAdjustment : Adjustment
    {
        public static readonly string Name = "Contrast";
        public ContrastAdjustmentItem ContrastAdjustmentItem=new ContrastAdjustmentItem();

        public ContrastAdjustment()
        {
            base.Type = AdjustmentType.Contrast;
            base.Icon = new ContrastControl();
            base.Item = this.ContrastAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}

