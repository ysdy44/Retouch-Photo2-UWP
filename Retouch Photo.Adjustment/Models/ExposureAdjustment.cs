using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
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

