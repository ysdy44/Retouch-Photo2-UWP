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
            ILayer layer = XML._createLayer(type, element);
            layer.Load(element);
            return layer;
            switch (type)
            {
                //Geometry0
                case "GeometryRectangle": return new GeometryRectangleLayer();
                case "GeometryEllipse": return new GeometryEllipseLayer();
                case "GeometryCurve": return new GeometryCurveLayer(element);

                case "TextArtistic": return new TextArtisticLayer(element);
                case "TextFrame": return new TextFrameLayer(element);

                case "Image": return new ImageLayer(element);
                case "Acrylic": return new AcrylicLayer(element);
                case "Group": return new GroupLayer();

                //Geometry1
                case "GeometryRoundRect": return new GeometryRoundRectLayer(element);
                case "GeometryTriangle": return new GeometryTriangleLayer(element);
                case "GeometryDiamond": return new GeometryDiamondLayer(element);

                //Geometry2
                case "GeometryPentagon": return new GeometryPentagonLayer(element);
                case "GeometryStar": return new GeometryStarLayer(element);
                case "GeometryCog": return new GeometryCogLayer(element);

                //Geometry3
                case "GeometryDount": return new GeometryDountLayer(element);
                case "GeometryPie": return new GeometryPieLayer(element);
                case "GeometryCookie": return new GeometryCookieLayer(element);

                //Geometry4
                case "GeometryArrow": return new GeometryArrowLayer(element);
                case "GeometryCapsule": return new GeometryCapsuleLayer();
                case "GeometryHeart": return new GeometryHeartLayer(element);

                default: return null;
            }
        }
        private static ILayer _createLayer(string type, XElement element)
        {
            switch (type)
            {
                //Geometry0
                case "GeometryRectangle": return new GeometryRectangleLayer();
                case "GeometryEllipse": return new GeometryEllipseLayer();
                case "GeometryCurve": return new GeometryCurveLayer();

                case "TextArtistic": return new TextArtisticLayer();
                case "TextFrame": return new TextFrameLayer();

                case "Image": return new ImageLayer();
                case "Acrylic": return new AcrylicLayer();
                case "Group": return new GroupLayer();

                //Geometry1
                case "GeometryRoundRect": return new GeometryRoundRectLayer();
                case "GeometryTriangle": return new GeometryTriangleLayer();
                case "GeometryDiamond": return new GeometryDiamondLayer();

                //Geometry2
                case "GeometryPentagon": return new GeometryPentagonLayer();
                case "GeometryStar": return new GeometryStarLayer();
                case "GeometryCog": return new GeometryCogLayer();

                //Geometry3
                case "GeometryDount": return new GeometryDountLayer();
                case "GeometryPie": return new GeometryPieLayer();
                case "GeometryCookie": return new GeometryCookieLayer();

                //Geometry4
                case "GeometryArrow": return new GeometryArrowLayer();
                case "GeometryCapsule": return new GeometryCapsuleLayer();
                case "GeometryHeart": return new GeometryHeartLayer();

                default: return null;
            }
        }

    }
}