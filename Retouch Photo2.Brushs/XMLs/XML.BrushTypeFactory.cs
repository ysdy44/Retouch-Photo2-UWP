// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a BrushType from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="BrushType"/>. </returns>
        public static BrushType CreateBrushType(string type)
        {
            switch (type)
            {
                case "None": return BrushType.None;
                case "Color": return BrushType.Color;
                case "LinearGradient": return BrushType.LinearGradient;
                case "RadialGradient": return BrushType.RadialGradient;
                case "EllipticalGradient": return BrushType.EllipticalGradient;
                case "Image": return BrushType.Image;

                default: return BrushType.None;
            }
        }

    }
}