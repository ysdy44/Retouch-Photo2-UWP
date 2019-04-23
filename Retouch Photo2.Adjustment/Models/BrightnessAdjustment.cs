using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;
using System.Numerics;

namespace Retouch_Photo2.Adjustments.Models
{
    public class BrightnessAdjustment : Adjustment
    {
        public static readonly string Name = "Brightness";
        public BrightnessAdjustmentItem BrightnessAdjustmentItem = new BrightnessAdjustmentItem();

        public BrightnessAdjustment()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            base.Item = this.BrightnessAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}

