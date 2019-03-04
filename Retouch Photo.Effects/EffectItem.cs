using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects
{
    public abstract class EffectItem
    {
        public bool IsOn;
        public abstract ICanvasImage Render(ICanvasImage image);
    }
}
