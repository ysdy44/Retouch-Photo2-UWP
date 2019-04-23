using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo2.Effects.Items
{
    public class GaussianBlurEffectItem : EffectItem
    {
        public float BlurAmount;

        public GaussianBlurEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.BlurAmount = 0;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new GaussianBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount
            };
        }
    }
}
