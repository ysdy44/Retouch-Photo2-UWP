using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class SaturationAdjustmentCandidate : AdjustmentCandidate
    {
        public SaturationPage page = new SaturationPage();

        public SaturationAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Saturation;
            base.Icon = new SaturationControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new SaturationAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is SaturationAdjustment SaturationAdjustment)
            {
                this.page.SaturationAdjustment = null;
                this.page.SaturationAdjustment = SaturationAdjustment;
            }
        }
    }
}

