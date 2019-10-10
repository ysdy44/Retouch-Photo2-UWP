using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public class GeometryPieLayer : IGeometryLayer
    {
        //@Construct
        public GeometryPieLayer()
        {
            base.Control.Icon = new GeometryPieIcon();
            base.Control.Text = "Pie";
        }

        //@Override       
        public override string Type => "Pie";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPieLayer PieLayer = new GeometryPieLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, PieLayer, this);
            return PieLayer;
        }
    }
}