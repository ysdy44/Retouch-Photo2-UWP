using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Controls.BlendControl;
using Windows.UI.Xaml;

namespace Retouch_Photo.Models.Blends
{
    public class LightenBlend : Blend
    {
        public LightenBlend()
        {
            base.Type = BlendType.Lighten;
        }

        protected override FrameworkElement GetIcon() => new BlendLightenControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Lighten
            };
        }
    }
}
