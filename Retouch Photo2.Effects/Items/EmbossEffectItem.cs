using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo2.Effects.Items
{
    public class EmbossEffectItem : EffectItem
    {
        public float Amount;
        public float Angle;

        public EmbossEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.Amount = 1;
            this.Angle = 0;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new EmbossEffect
            {
                Source = image,
                Amount = this.Amount,
                Angle = -this.Angle,
            };
        }
    }
}
