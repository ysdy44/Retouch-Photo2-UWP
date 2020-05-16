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
        /// <returns> The created <see cref="ILayer"/>. </returns>
        public static ILayer CreateLayer(string type)
        {
            switch (type)
            {
                //Geometry0
                case "GeometryRectangle": return new GeometryRectangleLayer();
                case "GeometryEllipse": return new GeometryEllipseLayer();

                case "Curve": return new CurveLayer();
                case "CurveMulti": return new CurveMultiLayer();

                case "TextArtistic": return new TextArtisticLayer();
                case "TextFrame": return new TextFrameLayer();

                case "Image": return new ImageLayer();
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

                default: return new GroupLayer();
            }
        }

    }
}