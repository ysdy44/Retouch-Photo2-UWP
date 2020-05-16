using Microsoft.Graphics.Canvas.Brushes;
using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="CanvasGradientStop"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="array"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveStop(string elementName, CanvasGradientStop stop)
        {
            return new XElement
            (
                elementName,
                new XAttribute("Position", stop.Position),
                FanKit.Transformers.XML.SaveColor("Color", stop.Color)
            );
        }

        /// <summary>
        ///  Loads a <see cref="CanvasGradientStop"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="CanvasGradientStop"/>. </returns>
        public static CanvasGradientStop LoadStop(XElement element)
        {
            CanvasGradientStop stop = new CanvasGradientStop();

            if (element.Attribute("Position") is XAttribute position) stop.Position = (float)position;
            if (element.Element("Color") is XElement color) stop.Color = FanKit.Transformers.XML.LoadColor(color);

            return stop;
        }

    }
}