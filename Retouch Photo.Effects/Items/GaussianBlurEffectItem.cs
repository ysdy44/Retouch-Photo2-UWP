using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects.Items
{
    public class GaussianBlurEffectItem : EffectItem
    {
        public float BlurAmount;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount
            };
        }
    }

}
