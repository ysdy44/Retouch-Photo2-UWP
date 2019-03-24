using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Effects.Items
{
    public class SharpenEffectItem : EffectItem
    {
        public float Amount;

        public SharpenEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.Amount = 0;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new SharpenEffect
            {
                Amount = this.Amount,
                Source = image
            };
        }
    }
}
