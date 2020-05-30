using Microsoft.Graphics.Canvas;
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
        /// <param name="customDevice"> The custom-device. </param>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="Layerage"/>. </returns>
        public static ILayer CreateLayer(CanvasDevice customDevice, string type)
        {
            switch (type)
            {
                //Geometry0
                case "GeometryRectangle": return new GeometryRectangleLayer(customDevice);
                case "GeometryEllipse": return new GeometryEllipseLayer(customDevice);

                case "Curve": return new CurveLayer(customDevice);
                case "CurveMulti": return new CurveMultiLayer(customDevice);

                case "TextArtistic": return new TextArtisticLayer(customDevice);
                case "TextFrame": return new TextFrameLayer(customDevice);

                case "Image": return new ImageLayer(customDevice);
                case "Group": return new GroupLayer(customDevice);

                //Geometry1
                case "GeometryRoundRect": return new GeometryRoundRectLayer(customDevice);
                case "GeometryTriangle": return new GeometryTriangleLayer(customDevice);
                case "GeometryDiamond": return new GeometryDiamondLayer(customDevice);

                //Geometry2
                case "GeometryPentagon": return new GeometryPentagonLayer(customDevice);
                case "GeometryStar": return new GeometryStarLayer(customDevice);
                case "GeometryCog": return new GeometryCogLayer(customDevice);

                //Geometry3
                case "GeometryDount": return new GeometryDountLayer(customDevice);
                case "GeometryPie": return new GeometryPieLayer(customDevice);
                case "GeometryCookie": return new GeometryCookieLayer(customDevice);

                //Geometry4
                case "GeometryArrow": return new GeometryArrowLayer(customDevice);
                case "GeometryCapsule": return new GeometryCapsuleLayer(customDevice);
                case "GeometryHeart": return new GeometryHeartLayer(customDevice);

                default: return new GroupLayer(customDevice);
            }
        }

    }
}