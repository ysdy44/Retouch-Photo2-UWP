// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Linq;
using Windows.Globalization;
using Windows.UI.Text;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Extensions of <see cref="Retouch_Photo2.Texts"/>.
    /// </summary>
    public static class TextsExtensions
    {

        /// <summary> Gets default font sizes. </summary>
        public static float DefaultFontSizes = 20f;

        /// <summary> Gets all font sizes. </summary>
        public static float[] FontSizes = new float[] { 5f, 6f, 7f, 8f, 9f, 10f, 11f, 12f, 13f, 14f, 15f, 16f, 18f, 20f, 24f, 30f, 36f, 48f, 64f, 72f, 96f, 144f, 288f };

        /// <summary>
        /// Match the closest font size.
        /// </summary>
        public static float MatchingFontSize(float fontSize)
        {
            if (TextsExtensions.FontSizes.Contains(fontSize)) return fontSize;

            float minSize = 5f;
            float minAbs = float.MaxValue;
            foreach (float size in TextsExtensions.FontSizes)
            {
                float abs = Math.Abs(size - fontSize);
                if (minAbs > abs)
                {
                    minSize = size;
                    minAbs = abs;
                }
            }

            return minSize;
        }

        /// <summary> Gets all font families. </summary>
        public static IOrderedEnumerable<string> FontFamilies = CanvasTextFormat.GetSystemFontFamilies(ApplicationLanguages.Languages).OrderBy(k => k);

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