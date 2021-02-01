// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Microsoft.Graphics.Canvas.Geometry;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {
                
        /// <summary>
        /// Create a Extend from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="CanvasCapStyle"/>. </returns>
        public static CanvasCapStyle CreateCap(string type)
        {
            switch (type)
            {
                case "Flat": return CanvasCapStyle.Flat;
                case "Square": return CanvasCapStyle.Square;
                case "Round": return CanvasCapStyle.Round;
                case "Triangle": return CanvasCapStyle.Triangle;

                default: return CanvasCapStyle.Flat;
            }
        }

    }
}