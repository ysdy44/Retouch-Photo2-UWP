using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendsControls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
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
