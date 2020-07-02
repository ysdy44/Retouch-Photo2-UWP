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
    /// <see cref="LayerBase"/>'s ArrowLayer .
    /// </summary>
    public class GeometryArrowLayer : LayerBase, ILayer
    {

        //@Override
        public override LayerType Type => LayerType.GeometryArrow;

        //@Content       
        public bool IsAbsolute = false;
        public float Width = 10;

        public float Value = 0.5f;
        public float StartingValue { get; private set; }
        public void CacheValue() => this.StartingValue = this.Value;

        public GeometryArrowTailType LeftTail = GeometryArrowTailType.None;
        public GeometryArrowTailType RightTail = GeometryArrowTailType.Arrow;

        //@Construct
        /// <summary>
        /// Initializes a arrow-layer.
        /// </summary>
        public GeometryArrowLayer(CanvasDevice customDevice)
        {
            base.Control = new LayerControl(customDevice, this)
            {
                Type = this.ConstructStrings(),
            };
        }


        public override ILayer Clone(CanvasDevice customDevice)
        {
            GeometryArrowLayer arrowLayer = new GeometryArrowLayer(customDevice)
            {
                IsAbsolute = this.IsAbsolute,
                Width = this.Width,
                Value = this.Value,

                LeftTail = this.LeftTail,
                RightTail = this.RightTail,
            };

            LayerBase.CopyWith(customDevice, arrowLayer, this);
            return arrowLayer;
        }
        
        public override void SaveWith(XElement element)
        {            
            element.Add(new XElement("IsAbsolute", this.IsAbsolute));
            element.Add(new XElement("Width", this.Width));
            element.Add(new XElement("Value", this.Value));

            element.Add(new XElement("LeftTail", this.LeftTail));
            element.Add(new XElement("RightTail", this.RightTail));
        }
        public override void Load(XElement element)
        {
            if (element.Element("IsAbsolute") is XElement isAbsolute) this.IsAbsolute = (bool)isAbsolute;
            if (element.Element("Width") is XElement width) this.Width = (float)width;
            if (element.Element("Value") is XElement value) this.Value = (float)value;

            if (element.Element("LeftTail") is XElement leftTail) this.LeftTail = FanKit.Transformers.XML.CreateGeometryArrowTailType(leftTail.Value);
            if (element.Element("RightTail") is XElement rightTail) this.RightTail = FanKit.Transformers.XML.CreateGeometryArrowTailType(rightTail.Value);
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateArrow(resourceCreator, transformer,
                this.IsAbsolute, this.Width, this.Value,
                this.LeftTail, this.RightTail);
        }
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            Transformer transformer = base.Transform.Transformer;

            return TransformerGeometry.CreateArrow(resourceCreator, transformer, matrix,
                this.IsAbsolute, this.Width, this.Value,
                this.LeftTail, this.RightTail);
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

            return resource.GetString("/Layers/GeometryArrow");
        }

    }
}