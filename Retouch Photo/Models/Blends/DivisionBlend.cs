using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendControl;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
{
    public class DivisionBlend : Blend
    {
        public DivisionBlend()
        {
            base.Type = BlendType.Division;
        }

        protected override FrameworkElement GetIcon() => new BlendDivisionControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Division
            };
        }
    }
}
