using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;

namespace Retouch_Photo.Adjustments.Models
{
    public class GrayAdjustmentCandidate : AdjustmentCandidate
    {
        public GrayAdjustmentCandidate()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new GrayControl();
            base.Page = null;
        }

        public override Adjustment GetNewAdjustment() => new GrayAdjustment();
        public override void SetPage(Adjustment adjustment) { }
    }
}
