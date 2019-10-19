using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryRectangleLayer .
    /// </summary>
    public class GeometryRectangleLayer : IGeometryLayer, ILayer
    {
        //@Content      
        public string Type => "GeometryRectangleLayer";

        //@Construct
        public GeometryRectangleLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRectangleIcon(),
                Text = "Rectangle",
            };
        }
        
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;
            
            return transformer.ToRectangle(resourceCreator, canvasToVirtualMatrix);            
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRectangleLayer rectangleLayer = new GeometryRectangleLayer();

            LayerBase.CopyWith(resourceCreator, rectangleLayer, this);
            return rectangleLayer;
        }

        public XElement Save()
        {
            XElement element = new XElement("GeometryRectangleLayer");

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}