using Retouch_Photo2.Adjustments.Controls;
using Retouch_Photo2.Adjustments.Items;

namespace Retouch_Photo2.Adjustments.Models
{
    /// <summary>
    /// <see cref="Adjustment"/>'s GammaTransferAdjustment.
    /// </summary>
    public class GammaTransferAdjustment : Adjustment
    {
        public const string Name = "GammaTransfer";
        public GammaTransferAdjustmentItem GammaTransferAdjustmentItem = new GammaTransferAdjustmentItem();

        //@Construct
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