using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendControl;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
{
    public class ScreenBlend : Blend
    {
        public ScreenBlend()
        {
            base.Type = BlendType.Screen;
        }

        protected override FrameworkElement GetIcon() => new BlendScreenControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Screen
            };
        }
    }
}
