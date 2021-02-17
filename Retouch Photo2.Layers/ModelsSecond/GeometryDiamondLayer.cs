// Core:              ★★★★
// Referenced:   ★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★
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
    /// <see cref="GeometryLayer"/>'s GeometryDiamondLayer .
    /// </summary>
    public class GeometryDiamondLayer : GeometryLayer, ILayer
    {

        //@Override     
        public override LayerType Type => LayerType.GeometryDiamond;

        //@Content
        public float Mid = 0.5f;
        public float StartingMid { get; private set; }
        public void CacheMid() => this.StartingMid = this.Mid;

               
        public override ILayer Clone()
        {
            GeometryDiamondLayer diamondLayer = new GeometryDiamondLayer
            {
                Mid = this.Mid
            };

            LayerBase.CopyWith(diamondLayer, this);
            return diamondLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("Mid", this.Mid));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Mid") is XElement mid) this.Mid = (float)mid;
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDiamond(resourceCreator, transformer, this.Mid);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateDiamond(resourceCreator, transformer, matrix, this.Mid);
        }


        //Strings
        private string ConstructStrings()
        {
            ResourceLoader resource = ResourceLoader.GetForCurrentView();

            return resource.GetString("Layers_GeometryDiamond");
        }

    }
}