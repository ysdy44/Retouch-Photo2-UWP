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
    /// <see cref="LayerBase"/>'s GeometryPentagonLayer .
    /// </summary>
    public class GeometryPentagonLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryPentagon;

        //@Content
        public int Points = 5;
        public int StartingPoints { get; private set; }
        public void CachePoints() => this.StartingPoints = this.Points;

        //@Construct
        /// <summary>
        /// Initializes a pentagon-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryPentagonLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryPentagonLayer pentagonLayer = new GeometryPentagonLayer(customDevice)
            {
                Points = this.Points,
            };

            LayerBase.CopyWith(customDevice, pentagonLayer, this);
            return pentagonLayer;
        }

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Points", this.Points));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Points") is XElement points) this.Points = (int)points;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, this.Points);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreatePentagon(resourceCreator, transformer, matrix, this.Points);
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

            return resource.GetString("/Layers/GeometryPentagon");
        }

    }
}