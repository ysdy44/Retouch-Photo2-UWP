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
    /// <see cref="IGeometryLayer"/>'s GeometryDountLayer .
    /// </summary>
    public partial class GeometryDountLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryDountLayer";
         
        //@Content       
        public float HoleRadius = 0.5f;

        //@Construct        
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryDountLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        public GeometryDountLayer()
        {
            base.Type = GeometryDountLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryDountIcon(),
                Text = "Dount",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, canvasToVirtualMatrix, this.HoleRadius);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDountLayer DountLayer = new GeometryDountLayer();

            LayerBase.CopyWith(resourceCreator, DountLayer, this);
            return DountLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("HoleRadius", this.HoleRadius));
        }
        public void Load(XElement element)
        {
            this.HoleRadius = (float)element.Element("HoleRadius");
        }

    }
}