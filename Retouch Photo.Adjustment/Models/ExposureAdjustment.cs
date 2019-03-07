using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class ExposureAdjustment : Adjustment
    {
        public ExposureAdjustmentItem ExposureAdjustmentItem=new ExposureAdjustmentItem();

        public ExposureAdjustment()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            base.Item = this.ExposureAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.ExposureAdjustmentItem.Exposure = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure=this.ExposureAdjustmentItem.Exposure,
                Source = image
            };
        } 
    }
}

