using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;
using Retouch_Photo.Adjustments.Pages;

namespace Retouch_Photo.Adjustments.Models
{
    public class TemperatureAdjustmentCandidate : AdjustmentCandidate
    {
        public TemperaturePage page = new TemperaturePage();

        public TemperatureAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Temperature;
            base.Icon = new TemperatureControl();
            base.Page = this.page;
        }

        public override Adjustment GetNewAdjustment() => new TemperatureAdjustment();
        public override void SetPage(Adjustment adjustment)
        {
            if (adjustment is TemperatureAdjustment TemperatureAdjustment)
            {
                this.page.TemperatureAdjustment = null;
                this.page.TemperatureAdjustment = TemperatureAdjustment;
            }
        }
    }
}

