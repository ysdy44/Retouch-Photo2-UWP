using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendControl;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
{
    public class DissolveBlend : Blend
    {
        public DissolveBlend()
        {
            base.Type = BlendType.Dissolve;
        }

        protected override FrameworkElement GetIcon() => new BlendDissolveControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Dissolve
            };
        }
    }
}
