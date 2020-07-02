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
    /// <see cref="LayerBase"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryHeart;

        //@Content
        public float Spread = 0.8f;
        public float StartingSpread { get; private set; }
        public void CacheSpread() => this.StartingSpread = this.Spread;

        //@Construct
        /// <summary>
        /// Initializes a heart-layer.
        /// </summary>
        public GeometryHeartLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryHeartLayer heartLayer = new GeometryHeartLayer(customDevice)
            {
                Spread = this.Spread
            };

            LayerBase.CopyWith(customDevice, heartLayer, this);
            return heartLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Spread", this.Spread));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Spread") is XElement spread) this.Spread = (float)spread;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, this.Spread);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, matrix, this.Spread);
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

            return resource.GetString("/Layers/GeometryHeart");
        }

    }
}