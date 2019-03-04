using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;
using Windows.UI;

namespace Retouch_Photo.Adjustments.Models
{
    public class VignetteAdjustmentCandidate : AdjustmentCandidate
    {
        public VignettePage page = new VignettePage();

        public VignetteAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Vignette;
            base.Icon = new VignetteControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new VignetteAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is VignetteAdjustment VignetteAdjustment)
            {
                this.page.VignetteAdjustment = null;
                this.page.VignetteAdjustment = VignetteAdjustment;
            }
        }
    }
}

