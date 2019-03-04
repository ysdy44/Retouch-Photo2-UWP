using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class ExposureAdjustmentCandidate : AdjustmentCandidate
    {
        public ExposurePage page = new ExposurePage();

        public ExposureAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new ExposureAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is ExposureAdjustment exposureAdjustment)
            {
                this.page.ExposureAdjustment = null;
                this.page.ExposureAdjustment = exposureAdjustment;
            }
        }
    }
}

