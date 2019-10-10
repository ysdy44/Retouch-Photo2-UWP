using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryStarLayer .
    /// </summary>
    public class GeometryStarLayer : IGeometryLayer
    {
        //@Construct
        public GeometryStarLayer()
        {
            base.Control.Icon = new GeometryStarIcon();
            base.Control.Text = "Star";
        }

        //@Override       
        public override string Type => "Star";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryStarLayer StarLayer = new GeometryStarLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, StarLayer, this);
            return StarLayer;
        }
    }
}