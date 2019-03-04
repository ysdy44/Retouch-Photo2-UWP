using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class GammaTransferAdjustmentCandidate : AdjustmentCandidate
    {
        public GammaTransferPage page = new GammaTransferPage();

        public GammaTransferAdjustmentCandidate()
        {
            base.Type = AdjustmentType.GammaTransfer;
            base.Icon = new GammaTransferControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new GammaTransferAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is GammaTransferAdjustment GammaTransferAdjustment)
            {
                this.page.GammaTransferAdjustment = null;
                this.page.GammaTransferAdjustment = GammaTransferAdjustment;
            }
        }
    }
}