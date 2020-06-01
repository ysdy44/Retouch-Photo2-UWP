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
    /// <see cref="LayerBase"/>'s GeometryRoundRectLayer .
    /// </summary>
    public class GeometryRoundRectLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryRoundRect;

        //@Content
        public float Corner = 0.25f;
        public float StartingCorner { get; private set; }
        public void CacheCorner() => this.StartingCorner = this.Corner;

        //@Construct
        /// <summary>
        /// Initializes a roundRect-layer.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        public GeometryRoundRectLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }
        

        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryRoundRectLayer roundRectLayer = new GeometryRoundRectLayer(customDevice)
            {
                Corner=this.Corner
            };

            LayerBase.CopyWith(customDevice, roundRectLayer, this);
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


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateRoundRect(resourceCreator, transformer, this.Corner);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateRoundRect(resourceCreator, transformer, canvasToVirtualMatrix, this.Corner);
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

            return resource.GetString("/Layers/GeometryRoundRect");
        }

    }
}