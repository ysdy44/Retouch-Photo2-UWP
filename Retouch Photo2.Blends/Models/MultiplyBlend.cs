using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends.Models
{
    public class MultiplyBlend : Blend
    {
        public MultiplyBlend()
        {
            base.Type = BlendType.Multiply;
        }

        protected override FrameworkElement GetIcon() => new MultiplyControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Multiply
            };
        }
    }
}
