using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Blends.Models
{
    public class LinearBurnBlend : Blend
    {
        public LinearBurnBlend()
        {
            base.Type = BlendType.LinearBurn;
        }

        protected override FrameworkElement GetIcon() => new LinearBurnControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.LinearBurn
            };
        }
    }
}
