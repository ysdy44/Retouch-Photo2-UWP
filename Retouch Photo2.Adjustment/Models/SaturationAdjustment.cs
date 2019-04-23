using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;

namespace Retouch_Photo2.Adjustments.Models
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

