using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentsControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class SaturationAdjustment : Adjustment
    {
        public float Saturation;

        public SaturationAdjustment()
        {
            base.Type = AdjustmentType.Saturation;
            base.Icon = new SaturationControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.Saturation = 1.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new SaturationEffect
            {
                Saturation = this.Saturation,
                Source = image
            };
        }
    }


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

