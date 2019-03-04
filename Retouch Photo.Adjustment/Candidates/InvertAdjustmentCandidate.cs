using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;

namespace Retouch_Photo.Adjustments.Models
{
    public class InvertAdjustmentCandidate : AdjustmentCandidate
    {
        public InvertAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            base.Page = null;
        }

        public override Adjustment GetNewAdjustment() => new InvertAdjustment();
        public override void SetPage(Adjustment adjustment) { }
    }
}
