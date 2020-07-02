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
    /// <see cref="LayerBase"/>'s GeometryDountLayer .
    /// </summary>
    public partial class GeometryDountLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDount;

        //@Content       
        public float HoleRadius = 0.5f;
        public float StartingHoleRadius { get; private set; }
        public void CacheHoleRadius() => this.StartingHoleRadius = this.HoleRadius;

        //@Construct
        /// <summary>
        /// Initializes a pie-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryDountLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryDountLayer dountLayer = new GeometryDountLayer(customDevice)
            {
                HoleRadius = this.HoleRadius,
            };

            LayerBase.CopyWith(customDevice, dountLayer, this);
            return dountLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("HoleRadius", this.HoleRadius));
        }
        public override void Load(XElement element)
        {
            if (element.Element("HoleRadius") is XElement holeRadius) this.HoleRadius = (float)holeRadius;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, this.HoleRadius);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDount(resourceCreator, transformer, matrix, this.HoleRadius);
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

            return resource.GetString("/Layers/GeometryDount");
        }

    }
}