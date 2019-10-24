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
    /// <see cref="IGeometryLayer"/>'s GeometryDiamondLayer .
    /// </summary>
    public class GeometryDiamondLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryDiamondLayer";
         
        //@Content
        public float Mid = 0.5f;

        //@Construct
        /// <summary>
        /// Construct a diamond-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryDiamondLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a diamond-layer.
        /// </summary>
        public GeometryDiamondLayer()
        {
            base.Type = GeometryDiamondLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryDiamondIcon(),
                Text = "Diamond",
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, canvasToVirtualMatrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, canvasToVirtualMatrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, canvasToVirtualMatrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, canvasToVirtualMatrix);

            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, canvasToVirtualMatrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, canvasToVirtualMatrix);

            Vector2 top = leftTop * (1.0f - this.Mid) + rightTop * this.Mid;
            Vector2 bottom = leftBottom * (1.0f - this.Mid) + rightBottom * this.Mid;

            //Points
            Vector2[] points = new Vector2[]
            {
                centerLeft,
                top,
                centerRight,
                bottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDiamondLayer DiamondLayer = new GeometryDiamondLayer();

            LayerBase.CopyWith(resourceCreator, DiamondLayer, this);
            return DiamondLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryDiamondLayer");
            
            element.Add(new XElement("Mid", this.Mid));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.Mid = (float)element.Element("Mid");
            LayerBase.LoadWith(element, this);
        }


    }
}