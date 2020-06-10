using Windows.UI.Text;

namespace Retouch_Photo2.Texts
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a FontStyle from an string and XElement.
        /// </summary>
        /// <param name="fontStyle"> The source string. </param>
        /// <returns> The created <see cref="FontStyle"/>. </returns>
        public static FontStyle CreateFontStyle(string fontStyle)
        {
            switch (fontStyle)
            {
                case "Normal": return FontStyle.Normal;
                case "Oblique": return FontStyle.Oblique;
                case "Italic": return FontStyle.Italic;

                default: return FontStyle.Italic;
            }
        }

    }
}