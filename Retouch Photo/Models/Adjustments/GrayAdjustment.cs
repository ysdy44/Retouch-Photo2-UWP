using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.AdjustmentControls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Adjustments
{
   public class GrayAdjustment: Adjustment
    {
        public GrayAdjustment()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new AdjustmentGrayControl();
            base.HasPage = false;
            this.Reset();
        }

        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
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
            base.Page = null;
        }

        public override Adjustment GetNewAdjustment() => new GrayAdjustment();
        public override void SetPage(Adjustment adjustment) { }
    }
}
