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
    /// <see cref="ILayer"/>'s GeometryDiamondLayer .
    /// </summary>
    public class GeometryDiamondLayer : LayerBase, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDiamond;

        //@Content
        public float Mid = 0.5f;

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
            GeometryDiamondLayer DiamondLayer = new GeometryDiamondLayer
            {
                Mid = this.Mid
            };

            LayerBase.CopyWith(resourceCreator, DiamondLayer, this);
            return DiamondLayer;
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
            Transformer transformer = base.Transform.Destination;

            return TransformerGeometry.CreateDiamond(resourceCreator, transformer, canvasToVirtualMatrix, this.Mid);
        }
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves()
        {
            Transformer transformer = base.Transform.Destination;

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