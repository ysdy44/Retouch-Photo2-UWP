using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="IGeometryLayer"/>'s GeometryTriangleLayer .
    /// </summary>
    public class GeometryTriangleLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryTriangle;

        //@Content
        public float Center = 0.5f;

        //@Construct     
        /// <summary>
        /// Construct a triangle-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryTriangleLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a triangle-layer.
        /// </summary>
        public GeometryTriangleLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryTriangleIcon(),
                Text = "Triangle",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, canvasToVirtualMatrix, this.Center);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryTriangleLayer TriangleLayer = new GeometryTriangleLayer
            {
                Center = this.Center,
            };

            LayerBase.CopyWith(resourceCreator, TriangleLayer, this);
            return TriangleLayer;
        }


        public void SaveWith(XElement element)
        {
            element.Add(new XElement("Center", this.Center));
        }
        public void Load(XElement element)
        {
            this.Center = (float)element.Element("Center");
        }

    }
}