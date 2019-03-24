using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo.Effects.Items
{
    public class StraightenEffectItem : EffectItem
    {
        public float Angle;

        public StraightenEffectItem()
        {
            this.Reset();
        }

        public override void Reset()
        {
            this.Angle = 0;
        }
        public override ICanvasImage GetRender(ICanvasImage image)
        {
            return new StraightenEffect
            {
                Angle = this.Angle,
                MaintainSize = true,
                Source = image,
            };
        }
    }
}
