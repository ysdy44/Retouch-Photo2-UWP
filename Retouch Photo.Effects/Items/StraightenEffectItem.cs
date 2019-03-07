using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects.Items
{
    public class StraightenEffectItem : EffectItem
    {
        public float Angle;

        public override ICanvasImage Render(ICanvasImage image)
        {
            return new Microsoft.Graphics.Canvas.Effects.StraightenEffect
            {
                Angle = this.Angle,
                MaintainSize = true,
                Source = image,
            };
        }
    }

}
