using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;

namespace Retouch_Photo.Models.Adjustments
{
    public class ExposureAdjustment : Adjustment
    {
        public ExposureAdjustment(AdjustmentCandidate candidate)
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new AdjustmentExposureControl();
            base.Candidate = candidate;
        }

        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new ExposureEffect
            {
                Exposure=1.0f,
                Source = image
            };
        }
    }


    public class ExposureAdjustmentCandidate : AdjustmentCandidate
    {
        public ExposureAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Exposure;
            base.Icon = new AdjustmentExposureControl();
        }
        public override Adjustment GetNewAdjustment() => new ExposureAdjustment(this);
    }
}

