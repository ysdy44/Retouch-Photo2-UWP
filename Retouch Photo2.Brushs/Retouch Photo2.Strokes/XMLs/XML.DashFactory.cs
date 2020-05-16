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
        /// <returns> The created <see cref="CanvasDashStyle"/>. </returns>
        public static CanvasDashStyle CreateDash(string type)
        {            
            switch (type)
            {
                case "Solid": return CanvasDashStyle.Solid;
                case "Dash": return CanvasDashStyle.Dash;
                case "Dot": return CanvasDashStyle.Dot;
                case "DashDot": return CanvasDashStyle.DashDot;
                case "DashDotDot": return CanvasDashStyle.DashDotDot;

                default: return CanvasDashStyle.Solid;
            }
        }

    }
}