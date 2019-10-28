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
    /// <see cref="IGeometryLayer"/>'s GeometryRoundRectLayer .
    /// </summary>
    public class GeometryRoundRectLayer : IGeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRoundRect;

        //@Content
        public float Corner = 0.25f;

        //@Construct   
        /// <summary>
        /// Construct a roundRect-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryRoundRectLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a roundRect-layer.
        /// </summary>
        public GeometryRoundRectLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRoundRectIcon(),
                Text = "RoundRect",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateRoundRect(resourceCreator, transformer, canvasToVirtualMatrix, this.Corner);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRoundRectLayer RoundRectLayer = new GeometryRoundRectLayer
            {
                Corner=this.Corner
            };

            LayerBase.CopyWith(resourceCreator, RoundRectLayer, this);
            return RoundRectLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("Corner", this.Corner));
        }
        public void Load(XElement element)
        {
            this.Corner = (float)element.Element("Corner");
        }
        
    }
}