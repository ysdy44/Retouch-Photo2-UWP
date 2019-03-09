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
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.SaturationAdjustmentItem.Saturation = 1.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.SaturationAdjustmentItem.Saturation,
                Source = image
            };
        }
    }
}

