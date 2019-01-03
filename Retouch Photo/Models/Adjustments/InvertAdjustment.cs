using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Adjustments
{
    public class InvertAdjustment : Adjustment
    {
        public InvertAdjustment()
        {
            base.Type = AdjustmentType.Invert;
            base.Icon = new InvertControl();
            base.HasPage = false;
            this.Reset();
        }

        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new InvertEffect { Source = image };
        }
    }


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
