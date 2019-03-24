using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class SaturationAdjustment : Adjustment
    {
        public static readonly string Name = "Saturation";
        public SaturationAdjustmentItem SaturationAdjustmentItem=new SaturationAdjustmentItem();

        public SaturationAdjustment()
        {
            base.Type = AdjustmentType.Saturation;
            base.Icon = new SaturationControl();
            base.Item = this.SaturationAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}

