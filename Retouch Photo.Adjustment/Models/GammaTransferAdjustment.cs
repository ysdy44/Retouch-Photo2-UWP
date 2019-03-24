using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class GammaTransferAdjustment : Adjustment
    {
        public static readonly string Name = "GammaTransfer";
        public GammaTransferAdjustmentItem GammaTransferAdjustmentItem = new GammaTransferAdjustmentItem();

        public GammaTransferAdjustment()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            base.Item = this.GammaTransferAdjustmentItem;
            base.Item.Reset();
            base.HasPage = true;
        }
    }
}