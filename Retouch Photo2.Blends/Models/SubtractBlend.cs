using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends.Models
{
    public class SubtractBlend : Blend
    {
        public SubtractBlend()
        {
            base.Type = BlendType.Subtract;
        }

        protected override FrameworkElement GetIcon() => new SubtractControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Subtract
            };
        }
    }
}
