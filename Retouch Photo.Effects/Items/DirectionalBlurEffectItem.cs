using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects.Items
{
    public class DirectionalBlurEffectItem : EffectItem
    {
        public float BlurAmount;
        public float Angle;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect
            {
                Source = image,
                BlurAmount = this.BlurAmount,
                Angle = -this.Angle,
            };
        }
    }
}
