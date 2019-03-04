using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class ContrastAdjustmentCandidate : AdjustmentCandidate
    {
        public ContrastPage page = new ContrastPage();

        public ContrastAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Contrast;
            base.Icon = new ContrastControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new ContrastAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is ContrastAdjustment ContrastAdjustment)
            {
                this.page.ContrastAdjustment = null;
                this.page.ContrastAdjustment = ContrastAdjustment;
            }
        }
    }
}

