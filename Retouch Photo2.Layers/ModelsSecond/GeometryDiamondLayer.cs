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
    /// <see cref="LayerBase"/>'s GeometryDiamondLayer .
    /// </summary>
    public class GeometryDiamondLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDiamond;

        //@Content
        public float Mid = 0.5f;
        public float StartingMid { get; private set; }
        public void CacheMid() => this.StartingMid = this.Mid;

        //@Construct
        /// <summary>
        /// Initializes a diamond-layer.
        /// </summary>
        public GeometryDiamondLayer()
        {
            base.Control = new LayerControl(this)
            {
                Icon = new GeometryDiamondIcon(),
                Type = this.ConstructStrings(),
            };
        }

       
        public override ILayer Clone(ICanvasResourceCreator resourceCreator)
        {
            GeometryDiamondLayer diamondLayer = new GeometryDiamondLayer
            {
                Mid = this.Mid
            };

            LayerBase.CopyWith(resourceCreator, diamondLayer, this);
            return diamondLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Mid", this.Mid));
        }
        public override void Load(XElement element)
        {
            this.Mid = (float)element.Element("Mid");
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDiamond(resourceCreator, transformer, canvasToVirtualMatrix, this.Mid);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.ConvertToCurvesFromDiamond(transformer, this.Mid);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("/Layers/GeometryDiamond");
        }

    }
}