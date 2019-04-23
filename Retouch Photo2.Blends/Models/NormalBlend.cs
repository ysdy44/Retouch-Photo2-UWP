using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Blends.Models
{
    public class NormalBlend : Blend
    {
        public NormalBlend()
        {
            base.Type = BlendType.Normal;
        }

        protected override FrameworkElement GetIcon() => new NormalControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new CompositeEffect
            {
                Sources =
                {
                    foreground,
                    background
                }
            };
        }
    }
}
