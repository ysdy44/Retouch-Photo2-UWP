using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Adjustments.Controls;

namespace Retouch_Photo.Adjustments.Models
{
    public class GrayAdjustment: Adjustment
    {
        public GrayAdjustment()
        {
            base.Type = AdjustmentType.Gray;
            base.Icon = new GrayControl();
            base.HasPage = false;
            this.Reset();
        }

        public override void Reset() { }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new GrayscaleEffect { Source = image };
        }
    }
}
