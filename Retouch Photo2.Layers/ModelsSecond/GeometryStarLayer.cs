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
    /// <see cref="IGeometryLayer"/>'s GeometryStarLayer .
    /// </summary>
    public class GeometryStarLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryStarLayer";

        //@Content       
        public int Points = 5;
        public float InnerRadius = 0.38f;

        //@Construct
        /// <summary>
        /// Construct a star-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryStarLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a star-layer.
        /// </summary>
        public GeometryStarLayer()
        {
            base.Type = GeometryStarLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryStarIcon(),
                Text = "Star",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(GeometryUtil.OneTransformer, base.TransformManager.Destination);
            Matrix3x2 matrix = oneMatrix * canvasToVirtualMatrix;

            float rotation = GeometryUtil.StartingRotation;
            float angle = FanKit.Math.Pi / this.Points;

            Vector2[] points = new Vector2[this.Points * 2];
            for (int i = 0; i < this.Points; i++)
            {
                int index = i * 2;

                //Outer
                Vector2 outer = GeometryUtil.GetRotationVector(rotation);
                points[index] = Vector2.Transform(outer, matrix);
                rotation += angle;

                //Inner
                Vector2 inner = GeometryUtil.GetRotationVector(rotation);
                points[index + 1] = Vector2.Transform(inner * this.InnerRadius, matrix);
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryStarLayer StarLayer = new GeometryStarLayer
            {
                Points=this.Points,
                InnerRadius= this.InnerRadius,
            };

            LayerBase.CopyWith(resourceCreator, StarLayer, this);
            return StarLayer;
        }


        public XElement Save()
        {
            XElement element = new XElement("GeometryStarLayer");
            
            element.Add(new XElement("Points", this.Points));
            element.Add(new XElement("InnerRadius", this.InnerRadius));

            LayerBase.SaveWidth(element, this);
            return element;
        }
        public void Load(XElement element)
        {
            this.Points = (int)element.Element("Points");
            this.InnerRadius = (float)element.Element("InnerRadius");
            LayerBase.LoadWith(element, this);
        }
        
    }
}