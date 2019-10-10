using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryEllipseLayer .
    /// </summary>
    public class GeometryEllipseLayer : IGeometryLayer
    {
        //@Construct
        public GeometryEllipseLayer()
        {
            base.Control.Icon = new GeometryEllipseIcon();
            base.Control.Text = "Ellipse";
        }

        //@Override       
        public override string Type => "Ellipse";
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.GetActualDestinationWithRefactoringTransformer;
            return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryEllipseLayer ellipseLayer= new GeometryEllipseLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, ellipseLayer, this);
            return ellipseLayer;
        }
    }
}