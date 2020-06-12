using System.Xml.Linq;
using Windows.UI.Text;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Turn into string.
        /// </summary>
        /// <param name="fontWeight"> The font weight. </param>
        /// <returns> The product string. </returns>
        public static string ToWeightsString(this FontWeight fontWeight)
        {
            ushort weight = fontWeight.Weight;

            if (weight == FontWeights.Black.Weight) return "Black";
            if (weight == FontWeights.Bold.Weight) return "Bold";

            if (weight == FontWeights.ExtraBlack.Weight) return "ExtraBlack";
            if (weight == FontWeights.ExtraBold.Weight) return "ExtraBold";
            if (weight == FontWeights.ExtraLight.Weight) return "ExtraLight";

            if (weight == FontWeights.Light.Weight) return "Light";
            if (weight == FontWeights.Medium.Weight) return "Medium";
            if (weight == FontWeights.Normal.Weight) return "Normal";

            if (weight == FontWeights.SemiBold.Weight) return "SemiBold";
            if (weight == FontWeights.SemiLight.Weight) return "SemiLight";

            if (weight == FontWeights.Thin.Weight) return "Thin";

            return "Normal";
        }

        /// <summary>
        /// Create a FontStyle from an string and XElement.
        /// </summary>
        /// <param name="fontWeight"> The source string. </param>
        /// <returns> The created <see cref="FontStyle"/>. </returns>
        public static FontWeight CreateFontWeight(string fontWeight)
        {
            switch (fontWeight)
            {
                case "Black": return FontWeights.Black;
                case "Bold": return FontWeights.Bold;

                case "ExtraBlack": return FontWeights.ExtraBlack;
                case "ExtraBold": return FontWeights.ExtraBold;
                case "ExtraLight": return FontWeights.ExtraLight;

                case "Light": return FontWeights.Light;
                case "Medium": return FontWeights.Medium;
                case "Normal": return FontWeights.Normal;

                case "SemiBold": return FontWeights.SemiBold;
                case "SemiLight": return FontWeights.SemiLight;

                case "Thin": return FontWeights.Thin;

                default: return FontWeights.Normal;
            }
        }
                   
    }
}