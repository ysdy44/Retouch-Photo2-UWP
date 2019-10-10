using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : IGeometryLayer
    {
        //@Construct
        public GeometryPentagonLayer()
        {
            base.Control.Icon = new GeometryPentagonIcon();
            base.Control.Text = "Pentagon";
        }

        //@Override       
        public override string Type => "Pentagon";

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPentagonLayer PentagonLayer = new GeometryPentagonLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, PentagonLayer, this);
            return PentagonLayer;
        }
    }
}