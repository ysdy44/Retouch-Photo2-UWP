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
    /// <see cref="IGeometryLayer"/>'s GeometryPieLayer .
    /// </summary>
    public partial class GeometryPieLayer : IGeometryLayer, ILayer
    {
        //@Static     
        public const string ID = "GeometryPieLayer";
         
        //@Content       
        public float SweepAngle = FanKit.Math.Pi / 2f;

        //@Construct        
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        public GeometryPieLayer(XElement element) : this() => this.Load(element);
        /// <summary>
        /// Construct a pie-layer.
        /// </summary>
        public GeometryPieLayer()
        {
            base.Type = GeometryPieLayer.ID;
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryPieIcon(),
                Text = "Pie",
            };
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.TransformManager.Destination;

            return TransformerGeometry.CreatePie(resourceCreator, transformer, canvasToVirtualMatrix, this.SweepAngle);
        }


        public ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPieLayer PieLayer = new GeometryPieLayer();

            LayerBase.CopyWith(resourceCreator, PieLayer, this);
            return PieLayer;
        }


        public void SaveWith(XElement element)
        {            
            element.Add(new XElement("SweepAngle", this.SweepAngle));
        }
        public void Load(XElement element)
        {
            this.SweepAngle = (float)element.Element("SweepAngle");
        }

    }
}