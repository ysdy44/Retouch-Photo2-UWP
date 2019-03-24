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

