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
using System;
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
        /// <summary> Gets or sets the font text. </summary>
        public abstract string FontText { get; set; }
        /// <summary> Gets or sets the font size. </summary>
        public abstract float FontSize { get; set; }
        /// <summary> Gets or sets the font family. </summary>
        public string FontFamily { get; set; } = "Arial";


        /// <summary> Gets or sets the underline. </summary>
        public bool Underline { get; set; }

        /// <summary> Gets or sets the font horizontal alignment. </summary>
        public CanvasHorizontalAlignment HorizontalAlignment { get; set; } = CanvasHorizontalAlignment.Left;
        /// <summary> Gets or sets the font style. </summary>
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;
        /// <summary> Gets or sets the font weight. </summary>
        public FontWeight2 FontWeight { get; set; } = FontWeight2.Normal;


        /// <summary>
        /// Saves the entire <see cref="ILayer"/> to a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        public override void SaveWith(XElement element)
        {
            element.Add(new XElement("Text", this.FontText));
            element.Add(new XElement("FontSize", this.FontSize));
            element.Add(new XElement("FontFamily", this.FontFamily));

            element.Add(new XElement("HorizontalAlignment", this.HorizontalAlignment));

            element.Add(new XElement("Underline", this.Underline));
            element.Add(new XElement("FontStyle", this.FontStyle));
            element.Add(new XElement("FontWeight", this.FontWeight));
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

            if (element.Element("HorizontalAlignment") is XElement horizontalAlignment)
            {
                try
                {
                    this.HorizontalAlignment = (CanvasHorizontalAlignment)Enum.Parse(typeof(CanvasHorizontalAlignment), horizontalAlignment.Value);
                }
                catch (Exception) { }
            }

            if (element.Element("Underline") is XElement underline) this.Underline = (bool)underline;
            if (element.Element("FontStyle") is XElement fontStyle)
            {
                try
                {
                    this.FontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontStyle.Value);
                }
                catch (Exception) { }
            }
            if (element.Element("FontWeight") is XElement fontWeight)
            {
                try
                {
                    this.FontWeight = (FontWeight2)Enum.Parse(typeof(FontWeight2), fontWeight.Value);
                }
                catch (Exception) { }
            }
        }


        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>   
        public override CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            Transformer transformer = base.Transform.Transformer;

            if (string.IsNullOrEmpty(this.FontText)) return CanvasGeometry.CreateText(new CanvasTextLayout(resourceCreator, string.Empty, new CanvasTextFormat(), 0, 0));

            using (CanvasTextFormat textFormat = new CanvasTextFormat
            {
                FontSize = this.FontSize,
                FontFamily = this.FontFamily,

                HorizontalAlignment = this.HorizontalAlignment,
                FontStyle = this.FontStyle,
                FontWeight = this.FontWeight.ToFontWeight(),
            })
            {
                float width = transformer.Horizontal.Length();
                if (width < 1 || float.IsNaN(width)) width = 10;
                float height = transformer.Vertical.Length();
                if (height < 1 || float.IsNaN(height)) height = 10;
                using (CanvasTextLayout textLayout = new CanvasTextLayout(resourceCreator, this.FontText, textFormat, width, height))
                {
                    int fontLength = this.FontText.Length;
                    if (this.Underline) textLayout.SetUnderline(0, fontLength, true);


                    TransformerRect rect = new TransformerRect(width, height, Vector2.Zero);
                    Matrix3x2 matrix = Transformer.FindHomography(rect, transformer);
                    CanvasGeometry geometry = CanvasGeometry.CreateText(textLayout).Transform(matrix);


                    return geometry;
                }
            }
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
        /// <param name="source"> The source <see cref="ITextLayer"/>. </param>
        /// <param name="destination"> The destination <see cref="ITextLayer"/>. </param>
        public static ITextLayer FontCopyWith(ITextLayer source, ITextLayer destination)
        {
            destination.FontText = source.FontText;
            destination.FontSize = source.FontSize;
            destination.FontFamily = source.FontFamily;

            destination.HorizontalAlignment = source.HorizontalAlignment;

            destination.Underline = source.Underline;
            destination.FontStyle = source.FontStyle;
            destination.FontWeight = source.FontWeight;

            return destination;
        }

    }
}