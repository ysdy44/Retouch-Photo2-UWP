using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;
using Windows.UI;

namespace Retouch_Photo.Adjustments.Models
{
    public class VignetteAdjustment : Adjustment
    {
        public VignetteAdjustmentItem VignetteAdjustmentItem = new VignetteAdjustmentItem();

        public VignetteAdjustment()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.Item = this.VignetteAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.VignetteAdjustmentItem.Amount = 0.0f;
            this.VignetteAdjustmentItem.Curve = 0.0f;
            this.VignetteAdjustmentItem.Color = Colors.Black;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new VignetteEffect
            {
                Amount = this.VignetteAdjustmentItem.Amount,
                Curve = this.VignetteAdjustmentItem.Curve,
                Color = this.VignetteAdjustmentItem.Color,
                Source = image
            };
        }
    }
}

