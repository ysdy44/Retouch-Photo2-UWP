using Retouch_Photo2.Layers.Models;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a PatternGridMode from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created PatternGridMode. </returns>
        public static PatternGridType CreatePatternGridMode(string type)
        {
            switch (type)
            {
                case "Horizontal": return PatternGridType.Horizontal;
                case "Vertical": return PatternGridType.Vertical;
                default: return PatternGridType.Grid;
            }
        }

    }
}