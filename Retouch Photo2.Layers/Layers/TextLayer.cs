// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★
// Complete:      ★★★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Retouch_Photo2.Texts;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Text;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="GeometryLayer"/>'s TextLayer.
    /// </summary>
    public abstract class TextLayer : GeometryLayer
    {

        //@Content       
        /// <summary> Gets or sets the text. </summary>
        public string FontText { get; set; } = string.Empty;
        /// <summary> Gets or sets the size. </summary>
        public abstract float FontSize { get; set; }
        /// <summary> Gets or sets the FontFamily. </summary>
        public string FontFamily { get; set; } = "Arial";

        /// <summary> Gets or sets the HorizontalAlignment. </summary>
        public CanvasHorizontalAlignment FontAlignment { get; set; } = CanvasHorizontalAlignment.Left;
        /// <summary> Gets or sets the style. </summary>
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        /// <summary> Gets or sets the weight. </summary>
        public FontWeight FontWeight { get; set; } = new FontWeight
        {
            Weight = 100,
        };


        /// <summary>
        /// Saves the entire <see cref="ILayer"/> to a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Text", this.FontText));
            element.Add(new XElement("FontSize", this.FontSize));
            element.Add(new XElement("FontFamily", this.FontFamily));

            element.Add(new XElement("HorizontalAlignment", this.FontAlignment));
            element.Add(new XElement("FontStyle", this.FontStyle));
            element.Add(new XElement("FontWeight", this.FontWeight.ToWeightsString()));
        }
        /// <summary>
        /// Load the entire <see cref="ILayer"/> form a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        public override void Load(XElement element)
        {
            if (element.Element("Text") is XElement text) this.FontText = text.Value;
            if (element.Element("FontSize") is XElement fontSize) this.FontSize = (float)fontSize;
            if (element.Element("FontFamily") is XElement fontFamily) this.FontFamily = fontFamily.Value;

            if (element.Element("HorizontalAlignment") is XElement horizontalAlignment) this.FontAlignment = Retouch_Photo2.Texts.XML.CreateHorizontalAlignment(horizontalAlignment.Value);
            if (element.Element("FontStyle") is XElement fontStyle) this.FontStyle = Retouch_Photo2.Texts.XML.CreateFontStyle(fontStyle.Value);
            if (element.Element("FontWeight") is XElement fontWeight) this.FontWeight= Retouch_Photo2.Texts.XML.CreateFontWeight(fontWeight.Value);
        }


        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>   
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;


            CanvasTextFormat textFormat = new CanvasTextFormat
            {
                FontSize = this.FontSize,
                FontFamily = this.FontFamily,

                HorizontalAlignment = this.FontAlignment,
                FontStyle = this.FontStyle,
                FontWeight = this.FontWeight,
            };

            float width = transformer.Horizontal.Length();
            float height = transformer.Vertical.Length();
            TransformerRect rect = new TransformerRect(width, height, Vector2.Zero);
            Matrix3x2 matrix = Transformer.FindHomography(rect, transformer);

            CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, this.FontText, textFormat, width, height);
            CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout).Transform(matrix);


            return geometry;
        }
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>   
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            return this.CreateGeometry(resourceCreator).Transform(matrix);
        }



        /// <summary>
        /// Convert to curves layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product nodes. </returns>
        public override NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator);

            return new NodeCollection(geometry);
        }



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