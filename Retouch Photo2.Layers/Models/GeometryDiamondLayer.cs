using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryDiamondLayer .
    /// </summary>
    public class GeometryDiamondLayer : IGeometryLayer
    {
        //@Construct
        public GeometryDiamondLayer()
        {
            base.Control.Icon = new GeometryDiamondIcon();
            base.Control.Text = "Diamond";
        }

        //@Override       
        public override string Type => "Diamond";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;

            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, canvasToVirtualMatrix);
            Vector2 centerTop = Vector2.Transform(transformer.CenterTop, canvasToVirtualMatrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, canvasToVirtualMatrix);
            Vector2 centerBottom = Vector2.Transform(transformer.CenterBottom, canvasToVirtualMatrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                centerLeft,
                centerTop,
                centerRight,
                centerBottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDiamondLayer DiamondLayer = new GeometryDiamondLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, DiamondLayer, this);
            return DiamondLayer;
        }
    }
}