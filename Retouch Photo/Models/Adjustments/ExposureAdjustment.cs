using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;
using Retouch_Photo.Pages.AdjustmentPages;

namespace Retouch_Photo.Models.Adjustments
{
    public class ExposureAdjustment : Adjustment
    {
        public float Exposure;

        public ExposureAdjustment()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new ExposureControl();
            base.HasPage = true;
            this.Reset();
        }

        public override void Reset()
        {
            this.Exposure = 0.0f;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure=this.Exposure,
                Source = image
            };
        } 
    }


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

