using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryTriangleLayer .
    /// </summary>
    public class GeometryTriangleLayer : IGeometryLayer
    {
        public float Center = 0.5f;

        //@Construct
        public GeometryTriangleLayer()
        {
            base.Control.Icon = new GeometryTriangleIcon();
            base.Control.Text = "Triangle";
        }

        //@Override       
        public override string Type => "Triangle";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;

            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, canvasToVirtualMatrix);

            Vector2 center = leftTop * (1.0f - this.Center) + rightTop * this.Center;

            //Points
            Vector2[] points = new Vector2[]
            {
                leftBottom,
                center,
                rightBottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryTriangleLayer TriangleLayer = new GeometryTriangleLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,

                Center = this.Center,
            };

            LayerBase.CopyWith(resourceCreator, TriangleLayer, this);
            return TriangleLayer;
        }
    }
}