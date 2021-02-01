// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Microsoft.Graphics.Canvas.Effects;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a blend mode from the string.
        /// </summary>
        /// <param name="mode"> The source string. </param>
        /// <returns> The created type. </returns>
        public static BlendEffectMode? CreateBlendMode(string mode)
        {
            switch (mode)
            {
                case "None": return null;
                case "Multiply": return BlendEffectMode.Multiply;
                case "Screen": return BlendEffectMode.Screen;
                case "Dissolve": return BlendEffectMode.Dissolve;
                case "Darken": return BlendEffectMode.Darken;
                case "Lighten": return BlendEffectMode.Lighten;
                case "DarkerColor": return BlendEffectMode.DarkerColor;
                case "LighterColor": return BlendEffectMode.LighterColor;
                case "ColorBurn": return BlendEffectMode.ColorBurn;
                case "ColorDodge": return BlendEffectMode.ColorDodge;
                case "LinearBurn": return BlendEffectMode.LinearBurn;
                case "LinearDodge": return BlendEffectMode.LinearDodge;
                case "Overlay": return BlendEffectMode.Overlay;
                case "SoftLight": return BlendEffectMode.SoftLight;
                case "HardLight": return BlendEffectMode.HardLight;
                case "VividLight": return BlendEffectMode.VividLight;
                case "LinearLight": return BlendEffectMode.LinearLight;
                case "PinLight": return BlendEffectMode.PinLight;
                case "HardMix": return BlendEffectMode.HardMix;
                case "Difference": return BlendEffectMode.Difference;
                case "Exclusion": return BlendEffectMode.Exclusion;
                case "Hue": return BlendEffectMode.Hue;
                case "Saturation": return BlendEffectMode.Saturation;
                case "Color": return BlendEffectMode.Color;
                case "Luminosity": return BlendEffectMode.Luminosity;
                case "Subtract": return BlendEffectMode.Subtract;
                case "Division": return BlendEffectMode.Division;
                default: return null;
            }
        }

    }
}