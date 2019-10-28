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
    /// <see cref="IGeometryLayer"/>'s GeometryCogLayer .
    /// </summary>
    public class GeometryCogLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryCogLayer";

        //@Content
        public int Count = 8;
        public float InnerRadius = 0.7f;
        public float Tooth = 0.3f;
        public float Notch = 0.6f;

        //@Construct      
        /// <summary>
        /// Construct a cog-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryCogLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a cog-layer.
        /// </summary>
        public GeometryCogLayer()
        {
            base.Type = GeometryCogLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryCogIcon(),
                Text = "Cog",
            };
        }

        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateCog
            (
                resourceCreator, 
                transformer,
                canvasToVirtualMatrix,

                this.Count, 
                this.InnerRadius,

                this.Tooth,
                this.Notch
           );
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryCogLayer cogLayer = new GeometryCogLayer
            {
                Count = this.Count,
                InnerRadius = this.InnerRadius,
                Tooth = this.Tooth,
                Notch = this.Notch,
            };

            LayerBase.CopyWith(resourceCreator, cogLayer, this);
            return cogLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("Count", this.Count));
            element.Add(new XElement("InnerRadius", this.InnerRadius));
            element.Add(new XElement("Tooth", this.Tooth));
            element.Add(new XElement("Notch", this.Notch));
        }
        public void Load(XElement element)
        {
            this.Count = (int)element.Element("Count");
            this.InnerRadius = (float)element.Element("InnerRadius");
            this.Tooth = (float)element.Element("Tooth");
            this.Notch = (float)element.Element("Notch");
        }

    }
}