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

            return TransformerGeometry.CreateDiamond(resourceCreator, transformer, canvasToVirtualMatrix, this.Mid);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDiamondLayer DiamondLayer = new GeometryDiamondLayer
            {
                Mid = this.Mid
            };

            LayerBase.CopyWith(resourceCreator, DiamondLayer, this);
            return DiamondLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("Mid", this.Mid));
        }
        public void Load(XElement element)
        {
            this.Mid = (float)element.Element("Mid");
        }
        
    }
}