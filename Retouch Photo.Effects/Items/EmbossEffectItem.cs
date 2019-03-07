using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects.Items
{
    public class EmbossEffectItem : EffectItem
    {
        public float Amount = 1;
        public float Angle;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.EmbossEffect
            {
                Source = image,
                Amount = this.Amount,
                Angle = -this.Angle,
            };
        }
    }
}
