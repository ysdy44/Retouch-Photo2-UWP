using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;
using Retouch_Photo2.Adjustments.Pages;
using Windows.UI;

namespace Retouch_Photo2.Adjustments.Models
{
    public class VignetteAdjustment : Adjustment
    {
        public static readonly string Name = "Vignette";
        public VignetteAdjustmentItem VignetteAdjustmentItem = new VignetteAdjustmentItem();

        public VignetteAdjustment()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.Item = this.VignetteAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}

