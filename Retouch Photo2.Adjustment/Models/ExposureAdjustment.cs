using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;

namespace Retouch_Photo2.Adjustments.Models
{
    public class ExposureAdjustment : Adjustment
    {
        public static readonly string Name = "Exposure";
        public ExposureAdjustmentItem ExposureAdjustmentItem=new ExposureAdjustmentItem();

        public ExposureAdjustment()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            base.Item = this.ExposureAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}

