using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Controls;
using Retouch_Photo2.Layers.ILayer;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s RectangleLayer .
    /// </summary>
    public class RectangleLayer : IGeometryLayer
    {
        //@Construct
        public RectangleLayer()
        {
            base.Name = "Rectangle";
            base.Icon = new RectangleControl();
        }

        //@Override
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(base.TransformerMatrix.Destination.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(base.TransformerMatrix.Destination.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(base.TransformerMatrix.Destination.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(base.TransformerMatrix.Destination.LeftBottom, canvasToVirtualMatrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }
    }
}