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
    /// <see cref="Layer"/>'s GeometryHeartLayer .
    /// </summary>
    public class GeometryHeartLayer : Layer, ILayer
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
        public GeometryHeartLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryHeartIcon(),
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryHeartLayer heartLayer = new GeometryHeartLayer
            {
                Spread = this.Spread
            };

            Layer.CopyWith(resourceCreator, heartLayer, this);
            return heartLayer;
        }
        
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Spread", this.Spread));
        }
        public override void Load(XElement element)
        {
            this.Spread = (float)element.Element("Spread");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateHeart(resourceCreator, transformer, canvasToVirtualMatrix, this.Spread);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.ConvertToCurvesFromHeart(transformer, this.Spread);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryHeart");
        }

    }
}