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
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateStar(resourceCreator, transformer, canvasToVirtualMatrix, this.Points, this.InnerRadius);
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


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("Points", this.Points));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
        }
        public void Load(XElement element)
        {
            this.Points = (int)element.Element("Points");
            this.InnerRadius = (float)element.Element("InnerRadius");
        }
        
    }
}