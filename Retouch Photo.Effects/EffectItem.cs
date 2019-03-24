using Microsoft.Graphics.Canvas;

namespace Retouch_Photo.Effects
{
    /// <summary> This contains all an <see cref = "Effect" />'s information. </summary>
    public abstract class EffectItem
    {
        public bool IsOn;
        public abstract void Reset();
        public abstract ICanvasImage GetRender(ICanvasImage image);
    }
}
