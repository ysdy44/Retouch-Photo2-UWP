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
    /// <see cref="Layer"/>'s GeometryTriangleLayer .
    /// </summary>
    public class GeometryTriangleLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryTriangle;

        //@Content
        public float Center = 0.5f;

        //@Construct
        /// <summary>
        /// Initializes a triangle-layer.
        /// </summary>
        public GeometryTriangleLayer()
        {
            base.Control = new LayerControl(this.ToLayerage())
            {
                Icon = new GeometryTriangleIcon(),
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryTriangleLayer TriangleLayer = new GeometryTriangleLayer
            {
                Center = this.Center,
            };

            Layer.CopyWith(resourceCreator, TriangleLayer, this);
            return TriangleLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Center", this.Center));
        }
        public override void Load(XElement element)
        {
            this.Center = (float)element.Element("Center");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, canvasToVirtualMatrix, this.Center);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromTriangle(transformer, this.Center);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryTriangle");
        }

    }
}