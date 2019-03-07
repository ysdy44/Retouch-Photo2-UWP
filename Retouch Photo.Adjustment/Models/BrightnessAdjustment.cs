using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;
using System.Numerics;

namespace Retouch_Photo.Adjustments.Models
{
    public class BrightnessAdjustment : Adjustment
    {
        public BrightnessAdjustmentItem BrightnessAdjustmentItem = new BrightnessAdjustmentItem();

        public BrightnessAdjustment()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            base.Item = this.BrightnessAdjustmentItem;
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.BrightnessAdjustmentItem.WhiteLight = 1.0f;
            this.BrightnessAdjustmentItem.WhiteDark = 1.0f;
            this.BrightnessAdjustmentItem.BlackLight = 1.0f;
            this.BrightnessAdjustmentItem.BlackDark = 1.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new BrightnessEffect
            {
                WhitePoint = new Vector2
                (
                    x: this.BrightnessAdjustmentItem.WhiteLight,
                    y: this.BrightnessAdjustmentItem.WhiteDark
                ),
                BlackPoint = new Vector2
                (
                    x: this.BrightnessAdjustmentItem.BlackDark,
                    y: this.BrightnessAdjustmentItem.BlackLight
                ),
                Source = image
            };
        }
    }
}

