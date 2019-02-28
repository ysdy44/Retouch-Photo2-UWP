using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentsControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class TemperatureAdjustment : Adjustment
    {
        public float Temperature;
        public float Tint;
        
        public TemperatureAdjustment()
        {
            base.Type = AdjustmentType.Temperature;
            base.Icon = new TemperatureControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.Temperature = 0.0f;
            this.Tint = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new TemperatureAndTintEffect
            {
                Temperature = this.Temperature,
                Tint = this.Tint,
                Source = image
            };
        }
    }


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

