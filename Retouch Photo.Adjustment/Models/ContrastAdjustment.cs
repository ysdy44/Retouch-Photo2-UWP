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
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.ContrastAdjustmentItem.Contrast = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ContrastEffect
            {
                Contrast = this.ContrastAdjustmentItem.Contrast,
                Source = image
            };
        }
    }
}

