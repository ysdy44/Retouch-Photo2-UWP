using Microsoft.Graphics.Canvas.Effects;
using System.Xml.Linq;

namespace Retouch_Photo2.Blends
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {
        
        /// <summary>
        /// Create a blend type from the string.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created type. </returns>
        public static BlendEffectMode? CreateBlendType(string type)
        {
            switch (type)
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

        /// <summary>
        /// Saves the entire <see cref="BlendEffectMode?"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="type"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveBlendType(string elementName, BlendEffectMode? type)
        {
            if (type is BlendEffectMode blendType)
            {
                return new XElement(elementName, $"{blendType}");
            }
            else
            {
                return new XElement(elementName, "None");
            }
        }

    }
}