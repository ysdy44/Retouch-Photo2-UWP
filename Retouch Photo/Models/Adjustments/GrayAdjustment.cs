using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;

namespace Retouch_Photo.Models.Adjustments
{
   public class GrayAdjustment: Adjustment
    {
        public GrayAdjustment(AdjustmentCandidate candidate)
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new AdjustmentGrayControl();
            base.Candidate = candidate;
        }

        public override ICanvasImage GetAdjustment(ICanvasImage image)
        {
            return new GrayscaleEffect { Source = image };
        }
    }


    public class GrayAdjustmentCandidate : AdjustmentCandidate
    {
        public GrayAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new AdjustmentGrayControl();
        }
        public override Adjustment GetNewAdjustment() => new GrayAdjustment(this);
    }
}
