using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;

namespace Retouch_Photo.Models.Adjustments
{
    public class InvertAdjustment : Adjustment
    {
        public InvertAdjustment(AdjustmentCandidate candidate)
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new AdjustmentInvertControl();
            base.Candidate = candidate;
        }

        public override ICanvasImage GetAdjustment(ICanvasImage image)
        {
            return new InvertEffect { Source = image };
        }
    }


    public class InvertAdjustmentCandidate : AdjustmentCandidate
    {
        public InvertAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new AdjustmentInvertControl();
        }
        public override Adjustment GetNewAdjustment() => new InvertAdjustment(this);
    }
}
