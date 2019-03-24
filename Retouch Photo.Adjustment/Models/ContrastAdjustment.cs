using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
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

