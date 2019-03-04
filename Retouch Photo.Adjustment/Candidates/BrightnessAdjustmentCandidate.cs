using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;
using System.Numerics;

namespace Retouch_Photo.Adjustments.Models
{
    public class BrightnessAdjustmentCandidate : AdjustmentCandidate
    {
        public BrightnessPage page = new BrightnessPage();

        public BrightnessAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Brightness;
            base.Icon = new BrightnessControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new BrightnessAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is BrightnessAdjustment BrightnessAdjustment)
            {
                this.page.BrightnessAdjustment = null;
                this.page.BrightnessAdjustment = BrightnessAdjustment;
            }
        }
    }
}

