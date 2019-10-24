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
        //@Static     
        public const string ID = "GeometryEllipseLayer";

        //@Construct
        /// <summary>
        /// Construct a ellipse-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryEllipseLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a ellipse-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryEllipseLayer()
        {
            base.Type = GeometryEllipseLayer.ID;
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
        public void Load(XElement element)
        {
            LayerBase.LoadWith(element, this);
        }

    }
}