using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Layers.Icons;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Windows.ApplicationModel.Resources;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s GeometryRoundRectLayer .
    /// </summary>
    public class GeometryRoundRectLayer : Layer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRoundRect;

        //@Content
        public float Corner = 0.25f;

        //@Construct
        /// <summary>
        /// Initializes a roundRect-layer.
        /// </summary>
        public GeometryRoundRectLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryRoundRectIcon(),
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryRoundRectLayer roundRectLayer = new GeometryRoundRectLayer
            {
                Corner=this.Corner
            };

            Layer.CopyWith(resourceCreator, roundRectLayer, this);
            return roundRectLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Corner", this.Corner));
        }
        public override void Load(XElement element)
        {
            this.Corner = (float)element.Element("Corner");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreateRoundRect(resourceCreator, transformer, canvasToVirtualMatrix, this.Corner);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.ConvertToCurvesFromRoundRect(transformer, this.Corner);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryRoundRect");
        }

    }
}