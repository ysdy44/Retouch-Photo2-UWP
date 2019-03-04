using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class HueRotationAdjustmentCandidate : AdjustmentCandidate
    {
        public HueRotationPage page = new HueRotationPage();

        public HueRotationAdjustmentCandidate()
        {
            base.Type = AdjustmentType.HueRotation;
            base.Icon = new HueRotationControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new HueRotationAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is HueRotationAdjustment HueRotationAdjustment)
            {
                this.page.HueRotationAdjustment = null;
                this.page.HueRotationAdjustment = HueRotationAdjustment;
            }
        }
    }
}

