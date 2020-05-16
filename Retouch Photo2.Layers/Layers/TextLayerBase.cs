using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Text;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="ILayer"/>'s TextLayerBase .
    /// </summary>
    public abstract class TextLayerBase : LayerBase
    {
        
        //@Content       
        public string FontText { get; set; } = string.Empty;

        public abstract float FontSize { get; set; } 
        public string FontFamily { get; set; } = "Arial";

        public CanvasHorizontalAlignment FontAlignment { get; set; } = CanvasHorizontalAlignment.Left;
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        public FontWeight FontWeight { get; set; } = new FontWeight
        {
            Weight = 100,
        };
        

        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Text", this.FontText));
            element.Add(new XElement("FontSize", this.FontSize));
            element.Add(new XElement("FontFamily", this.FontFamily));

            element.Add(new XElement("HorizontalAlignment", this.FontAlignment));
            element.Add(new XElement("FontStyle", this.FontStyle));
            element.Add(new XElement("FontWeight", this.FontWeight.Weight));
        }
        public override void Load(XElement element)
        {
            if (element.Element("Text") is XElement text) this.FontText = text.Value;
            if (element.Element("FontSize") is XElement fontSize) this.FontSize = (float)fontSize;
            if (element.Element("FontFamily") is XElement fontFamily) this.FontFamily = fontFamily.Value;

            if (element.Element("HorizontalAlignment") is XElement horizontalAlignment)
            {
                switch (horizontalAlignment.Value)
                {
                    case "Left": this.FontAlignment = CanvasHorizontalAlignment.Left; break;
                    case "Right": this.FontAlignment = CanvasHorizontalAlignment.Right; break;
                    case "Center": this.FontAlignment = CanvasHorizontalAlignment.Center; break;
                    default: this.FontAlignment = CanvasHorizontalAlignment.Justified; break;
                }
            }
            if (element.Element("FontStyle") is XElement fontStyle)
            {
                switch (fontStyle.Value)
                {
                    case "Normal": this.FontStyle = FontStyle.Normal; break;
                    case "Oblique": this.FontStyle = FontStyle.Oblique; break;
                    default: this.FontStyle = FontStyle.Italic; break;
                }
            }
            if (element.Element("FontWeight") is XElement fontWeight)
            {
                this.FontWeight = new FontWeight
                {
                    Weight = (ushort)(int)fontWeight
                };
            }
        }


        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix)
        {
            Transformer transformer = base.Transform.Destination;


            float scale = (canvasToVirtualMatrix.M11 + canvasToVirtualMatrix.M22) / 2;
            CanvasTextFormat textFormat = new CanvasTextFormat
            {
                FontSize = this.FontSize * scale,
                FontFamily = this.FontFamily,

                HorizontalAlignment = this.FontAlignment,
                FontStyle = this.FontStyle,
                FontWeight = this.FontWeight,
            };

            float width = transformer.Horizontal.Length() * scale;
            float height = transformer.Vertical.Length() * scale;
            TransformerRect rect = new TransformerRect(width, height, Vector2.Zero);
            Matrix3x2 matrix = Transformer.FindHomography(rect, transformer) * canvasToVirtualMatrix;

            CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, this.FontText, textFormat, width, height);
            CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout).Transform(matrix);


            return geometry;
        }
        
        public override IEnumerable<IEnumerable<Node>> ConvertToCurves() => null;


        //@Static
        /// <summary>
        /// Copy a font-layer with self.
        /// </summary>
        /// <param name="source"> The source ITextLayer. </param>
        /// <param name="destination"> The destination ITextLayer. </param>
        public static void FontCopyWith(ITextLayer destination, ITextLayer source)
        {
            destination.FontText = source.FontText;
            destination.FontSize = source.FontSize;
            destination.FontFamily = source.FontFamily;

            destination.FontAlignment = source.FontAlignment;
            destination.FontStyle = source.FontStyle;
            destination.FontWeight = source.FontWeight;
        }

    }
}