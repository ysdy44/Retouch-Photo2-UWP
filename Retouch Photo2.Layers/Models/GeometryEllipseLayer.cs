using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryEllipseLayer .
    /// </summary>
    public class GeometryEllipseLayer : IGeometryLayer, ILayer
    {
        //@Content       
        public string Type => "GeometryEllipseLayer";

        //@Construct
        public GeometryEllipseLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryEllipseIcon(),
                Text = "Ellipse",
            };
        }
                
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return transformer.ToEllipse(resourceCreator, canvasToVirtualMatrix);
        }

        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryEllipseLayer ellipseLayer = new GeometryEllipseLayer();

            LayerBase.CopyWith(resourceCreator, ellipseLayer, this);
            return ellipseLayer;
        }

        public XElement Save()
        {
            XElement element = new XElement("GeometryEllipseLayer");

            LayerBase.SaveWidth(element, this);
            return element;
        }

    }
}