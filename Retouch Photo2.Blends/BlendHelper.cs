using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Provides static blend rendering method.
    /// </summary>
    public class BlendHelper
    {
        //@Static
        /// <summary>
        /// Render images and blend together.
        /// </summary>      
        /// <param name="background"> Background image. </param>
        /// <param name="foreground"> Foreground image. </param>
        /// <param name="type"> Type </param>
        /// <returns> The rendered blend. </returns>
        public static ICanvasImage Render(ICanvasImage background, ICanvasImage foreground, BlendType type)
        {
            if (type== BlendType.None)
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

            return new BlendEffect
            {
                Background = background,
                Foreground = foreground,
                Mode = BlendHelper.GetMode(type)
            };
        } 

        private static BlendEffectMode GetMode(BlendType type)
        {
            switch (type)
            {
                case BlendType.None: return BlendEffectMode.Multiply;
                case BlendType.Multiply: return BlendEffectMode.Multiply;
                case BlendType.Screen: return BlendEffectMode.Screen;
                case BlendType.Dissolve: return BlendEffectMode.Dissolve;
                case BlendType.Darken: return BlendEffectMode.Darken;
                case BlendType.Lighten: return BlendEffectMode.Lighten;
                case BlendType.DarkerColor: return BlendEffectMode.DarkerColor;
                case BlendType.LighterColor: return BlendEffectMode.LighterColor;
                case BlendType.ColorBurn: return BlendEffectMode.ColorBurn;
                case BlendType.ColorDodge: return BlendEffectMode.ColorDodge;
                case BlendType.LinearBurn: return BlendEffectMode.LinearBurn;
                case BlendType.LinearDodge: return BlendEffectMode.LinearDodge;
                case BlendType.Overlay: return BlendEffectMode.Overlay;
                case BlendType.SoftLight: return BlendEffectMode.SoftLight;
                case BlendType.HardLight: return BlendEffectMode.HardLight;
                case BlendType.VividLight: return BlendEffectMode.VividLight;
                case BlendType.LinearLight: return BlendEffectMode.LinearLight;
                case BlendType.PinLight: return BlendEffectMode.PinLight;
                case BlendType.HardMix: return BlendEffectMode.HardMix;
                case BlendType.Difference: return BlendEffectMode.Difference;
                case BlendType.Exclusion: return BlendEffectMode.Exclusion;
                case BlendType.Hue: return BlendEffectMode.Hue;
                case BlendType.Saturation: return BlendEffectMode.Saturation;
                case BlendType.Color: return BlendEffectMode.Color;
                case BlendType.Luminosity: return BlendEffectMode.Luminosity;
                case BlendType.Subtract: return BlendEffectMode.Subtract;
                case BlendType.Division: return BlendEffectMode.Division;
                default: return BlendEffectMode.Multiply;
            }
        }
    }
}