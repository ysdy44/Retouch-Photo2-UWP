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
        public static BlendType CreateBlendType(string type)
        {
            switch (type)
            {
                case "None": return BlendType.None;
                case "Multiply": return BlendType.Multiply;
                case "Screen": return BlendType.Screen;
                case "Dissolve": return BlendType.Dissolve;
                case "Darken": return BlendType.Darken;
                case "Lighten": return BlendType.Lighten;
                case "DarkerColor": return BlendType.DarkerColor;
                case "LighterColor": return BlendType.LighterColor;
                case "ColorBurn": return BlendType.ColorBurn;
                case "ColorDodge": return BlendType.ColorDodge;
                case "LinearBurn": return BlendType.LinearBurn;
                case "LinearDodge": return BlendType.LinearDodge;
                case "Overlay": return BlendType.Overlay;
                case "SoftLight": return BlendType.SoftLight;
                case "HardLight": return BlendType.HardLight;
                case "VividLight": return BlendType.VividLight;
                case "LinearLight": return BlendType.LinearLight;
                case "PinLight": return BlendType.PinLight;
                case "HardMix": return BlendType.HardMix;
                case "Difference": return BlendType.Difference;
                case "Exclusion": return BlendType.Exclusion;
                case "Hue": return BlendType.Hue;
                case "Saturation": return BlendType.Saturation;
                case "Color": return BlendType.Color;
                case "Luminosity": return BlendType.Luminosity;
                case "Subtract": return BlendType.Subtract;
                case "Division": return BlendType.Division;
                default: return BlendType.None;
            }
        }

    }
}