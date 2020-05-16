using Microsoft.Graphics.Canvas.Text;

namespace Retouch_Photo2.Characters
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {
        
        /// <summary>
        /// Create a blend mode from the string.
        /// </summary>
        /// <param name="mode"> The source string. </param>
        /// <returns> The created type. </returns>
        public static CanvasHorizontalAlignment CreateHorizontalAlignment(string horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case "Left": return CanvasHorizontalAlignment.Left; 
                case "Right": return CanvasHorizontalAlignment.Right; 
                case "Center": return CanvasHorizontalAlignment.Center; 
                default: return CanvasHorizontalAlignment.Justified; 
            }
        }
        
    }
}