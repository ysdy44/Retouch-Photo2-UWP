using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : IGeometryLayer
    {
        //@Construct
        public GeometryRectangleLayer()
        {
            base.Control.Icon = new GeometryRectangleIcon();
            base.Control.Text = "Rectangle";
        }

        //@Override      
        public override string Type => "Rectangle";
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRectangleLayer rectangleLayer = new GeometryRectangleLayer
            {
                FillBrush = base.FillBrush,
                StrokeBrush = base.StrokeBrush,
            };

            LayerBase.CopyWith(resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }
    }
}