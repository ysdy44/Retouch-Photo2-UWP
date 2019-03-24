using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Effects.Items
{
    public class DirectionalBlurEffectItem : EffectItem
    {
        public float BlurAmount;
        public float Angle;

        public DirectionalBlurEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.BlurAmount = 0;
            this.Angle = 0;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new DirectionalBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount,
                Angle = -this.Angle,
            };
        }
    }
}
