// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Windows.UI.Text;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Extensions of <see cref = "FontWeight2" />.
    /// </summary>
    public static class FontWeightExtensions
    {

        /// <summary>
        /// Turn into font wieght.
        /// </summary>
        /// <param name="fontWeight"> The source font wieght. </param>
        /// <returns> The product font wieght. </returns>
        public static FontWeight ToFontWeight(this FontWeight2 fontWeight)
        {
            switch (fontWeight)
            {
                case FontWeight2.Black: return FontWeights.Black;
                case FontWeight2.Bold: return FontWeights.Bold;

                case FontWeight2.ExtraBlack: return FontWeights.ExtraBlack;
                case FontWeight2.ExtraBold: return FontWeights.ExtraBold;
                case FontWeight2.ExtraLight: return FontWeights.ExtraLight;

                case FontWeight2.Light: return FontWeights.Light;
                case FontWeight2.Medium: return FontWeights.Medium;
                case FontWeight2.Normal: return FontWeights.Normal;

                case FontWeight2.SemiBold: return FontWeights.SemiBold;
                case FontWeight2.SemiLight: return FontWeights.SemiLight;

                case FontWeight2.Thin: return FontWeights.Thin;

                default: return FontWeights.Normal;
            }
        }


    }
}