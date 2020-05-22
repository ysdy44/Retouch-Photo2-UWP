using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GeometryPieLayer .
    /// </summary>
    public partial class GeometryPieLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPie;

        //@Content       
        public float SweepAngle = FanKit.Math.Pi / 2f;

        //@Construct
        /// <summary>
        /// Initializes a pie-layer.
        /// </summary>
        public GeometryPieLayer()
        {
            base.Control = new LayerControl(this.ToLayerage())
            {
                Icon = new GeometryPieIcon(),
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryPieLayer PieLayer = new GeometryPieLayer();

            Layer.CopyWith(resourceCreator, PieLayer, this);
            return PieLayer;
        }

        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("SweepAngle", this.SweepAngle));
        }
        public override void Load(XElement element)
        {
            this.SweepAngle = (float)element.Element("SweepAngle");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreatePie(resourceCreator, transformer, canvasToVirtualMatrix, this.SweepAngle);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromPie(transformer, this.SweepAngle);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryPie");
        }

    }
}