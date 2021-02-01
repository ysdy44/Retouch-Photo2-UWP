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
        /// <returns> The created <see cref="CanvasLineJoin"/>. </returns>
        public static CanvasLineJoin CreateJoin(string type)
        {
            switch (type)
            {
                case "Miter": return CanvasLineJoin.Miter;
                case "Bevel": return CanvasLineJoin.Bevel;
                case "Round": return CanvasLineJoin.Round;
                case "MiterOrBevel": return CanvasLineJoin.MiterOrBevel;

                default: return CanvasLineJoin.Miter;
            }
        }

    }
}