using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo.Blends.Controls;
using Windows.UI.Xaml;

namespace Retouch_Photo.Blends.Models
{
    public class ExclusionBlend : Blend
    {
        public ExclusionBlend()
        {
            base.Type = BlendType.Exclusion;
        }

        protected override FrameworkElement GetIcon() => new ExclusionControl();
        protected override ICanvasImage GetRender(ICanvasImage background, ICanvasImage foreground)
        {
            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendEffectMode.Exclusion
            };
        }
    }
}
