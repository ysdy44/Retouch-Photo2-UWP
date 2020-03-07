using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Brush"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="brush"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        private static XElement SaveBrush(string elementName, Brush brush)
        {
            switch (brush.Type)
            {
                case BrushType.None:
                    return new XElement
                    (
                        elementName,
                        new XElement("Type", BrushType.None)
                    );
                case BrushType.Color:
                    return new XElement
                   (
                       elementName,
                       new XElement("Type", BrushType.Color),
                       FanKit.Transformers.XML.SaveColor("Color", brush.Color)
                   );
                case BrushType.LinearGradient:
                    return new XElement
                    (
                       elementName,
                       new XElement("Type", BrushType.LinearGradient),
                       XML.SaveCanvasGradientStopArray("Array", brush.Array),
                       XML.SaveBrushPoints("Points", brush.Points, GradientBrushType.Linear)
                    );
                case BrushType.RadialGradient:
                    return new XElement
                    (
                       elementName,
                       new XElement("Type", BrushType.RadialGradient),
                       XML.SaveCanvasGradientStopArray("Array", brush.Array),
                       XML.SaveBrushPoints("Points", brush.Points, GradientBrushType.Radial)
                    );
                case BrushType.EllipticalGradient:
                    return new XElement
                    (
                       elementName,
                       new XElement("Type", BrushType.EllipticalGradient),
                       XML.SaveCanvasGradientStopArray("Array", brush.Array),
                       XML.SaveBrushPoints("Points", brush.Points, GradientBrushType.Elliptical)
                    );
                case BrushType.Image:
                    return new XElement
                    (
                       elementName,
                       new XElement("Type", BrushType.Image),
                       Retouch_Photo2.Elements.XML.SaveImageStr("ImageStr", brush.ImageStr),
                       FanKit.Transformers.XML.SaveTransformer("ImageSource", brush.ImageSource),
                       FanKit.Transformers.XML.SaveTransformer("ImageDestination", brush.ImageDestination)
                    );
                default:
                    return new XElement
                    (
                       elementName,
                       new XElement("Type", BrushType.None)
                    );
            }
        }
        /// <summary>
        ///  Loads a <see cref="Brush"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Brush"/>. </returns>
        private static Brush LoadBrush(XElement element)
        {
            var type = element.Element("Type").Value;
            switch (type)
            {
                case "None":
                    return new Brush()
                    {
                        Type = BrushType.None,
                    };
                case "Color":
                    return new Brush()
                    {
                        Type = BrushType.Color,
                        Color = FanKit.Transformers.XML.LoadColor(element.Element("Color")),
                    };
                case "LinearGradient":
                    return new Brush()
                    {
                        Type = BrushType.LinearGradient,
                        Array = XML.LoadCanvasGradientStopArray(element.Element("Array")),
                        Points = XML.LoadBrushPoints(element.Element("Points"), GradientBrushType.Linear)
                    };
                case "RadialGradient":
                    return new Brush()
                    {
                        Type = BrushType.RadialGradient,
                        Array = XML.LoadCanvasGradientStopArray(element.Element("Array")),
                        Points = XML.LoadBrushPoints(element.Element("Points"), GradientBrushType.Radial)
                    };
                case "EllipticalGradient":
                    return new Brush()
                    {
                        Type = BrushType.EllipticalGradient,
                        Array = XML.LoadCanvasGradientStopArray(element.Element("Array")),
                        Points = XML.LoadBrushPoints(element.Element("Points"), GradientBrushType.Elliptical)
                    };
                case "Image":
                    return new Brush
                    {
                        Type = BrushType.Image,
                        ImageStr = Retouch_Photo2.Elements.XML.LoadImageStr(element.Element("ImageStr")),
                        ImageSource = FanKit.Transformers.XML.LoadTransformer(element.Element("ImageSource")),
                        ImageDestination = FanKit.Transformers.XML.LoadTransformer(element.Element("ImageDestination")),
                    };
                default:
                    return new Brush
                    {
                        Type = BrushType.None,
                    };
            }
        }

    }
}