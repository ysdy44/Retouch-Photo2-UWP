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
        //@Static     
        public const string ID = "GeometryTriangleLayer";

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
            base.Type = GeometryTriangleLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryTriangleIcon(),
                Text = "Triangle",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, canvasToVirtualMatrix);

            Vector2 center = leftTop * (1.0f - this.Center) + rightTop * this.Center;

            //Points
            Vector2[] points = new Vector2[]
            {
                leftBottom,
                center,
                rightBottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
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


        public XElement Save()
        {
            XElement element = new XElement("GeometryTriangleLayer");

            element.Add(new XElement("Center", this.Center));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.Center = (float)element.Element("Center");
            LayerBase.LoadWith(element, this);
        }

    }
}