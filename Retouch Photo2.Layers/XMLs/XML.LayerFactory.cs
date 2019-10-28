using Retouch_Photo2.Layers.Models;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a Layer from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The created Layer. </returns>
        public static ILayer CreateLayer(string type, XElement element)
        {
            switch (type)
            {
                case GeometryRectangleLayer.ID: return new GeometryRectangleLayer();
                case GeometryEllipseLayer.ID: return new GeometryEllipseLayer();
                case GeometryCurveLayer.ID: return new GeometryCurveLayer(element);

                case ImageLayer.ID: return new ImageLayer(element);
                case AcrylicLayer.ID: return new AcrylicLayer(element);
                case GroupLayer.ID: return new GroupLayer();

                case GeometryRoundRectLayer.ID: return new GeometryRoundRectLayer(element);
                case GeometryTriangleLayer.ID: return new GeometryTriangleLayer(element);

                case GeometryDiamondLayer.ID: return new GeometryDiamondLayer(element);
                case GeometryPentagonLayer.ID: return new GeometryPentagonLayer(element);
                case GeometryStarLayer.ID: return new GeometryStarLayer(element);
                case GeometryPieLayer.ID: return new GeometryPieLayer(element);

                case GeometryCogLayer.ID: return new GeometryCogLayer(element);
                case GeometryArrowLayer.ID: return new GeometryArrowLayer(element);
                case GeometryCapsuleLayer.ID: return new GeometryCapsuleLayer();
                case GeometryHeartLayer.ID: return new GeometryHeartLayer(element);

                default: return null;
            }
        }

    }
}