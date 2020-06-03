using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s GeometryTriangleLayer .
    /// </summary>
    public class GeometryTriangleLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryTriangle;

        //@Content
        public float Center = 0.5f;
        public float StartingCenter { get; private set; }
        public void CacheCenter() => this.StartingCenter = this.Center;

        //@Construct
        /// <summary>
        /// Initializes a triangle-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryTriangleLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryTriangleLayer triangleLayer = new GeometryTriangleLayer(customDevice)
            {
                Center = this.Center,
            };

            LayerBase.CopyWith(customDevice, triangleLayer, this);
            return triangleLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Center", this.Center));
        }
        public override void Load(XElement element)
        {
            this.Center = (float)element.Element("Center");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, this.Center);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateTriangle(resourceCreator, transformer, matrix, this.Center);
        }


        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            return new NodeCollection(geometry);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryTriangle");
        }

    }
}