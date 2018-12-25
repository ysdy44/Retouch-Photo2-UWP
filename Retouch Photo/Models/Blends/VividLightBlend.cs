using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendControl;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
{
    public class VividLightBlend : Blend
    {
        public VividLightBlend()
        {
            base.Type = BlendType.VividLight;
        }

        protected override FrameworkElement GetIcon() => new BlendVividLightControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.VividLight
            };
        }
    }
}
