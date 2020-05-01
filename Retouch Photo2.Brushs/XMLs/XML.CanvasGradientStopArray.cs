using Microsoft.Graphics.Canvas.Brushes;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="CanvasGradientStop[]"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="array"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveCanvasGradientStopArray(string elementName, CanvasGradientStop[] array)
        {
            return new XElement
            (
                elementName,
                from stop
                in array
                select new XElement
                (
                    "Stop",
                    new XAttribute("Position", stop.Position),
                    FanKit.Transformers.XML.SaveColor("Color", stop.Color)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="CanvasGradientStop[]"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="CanvasGradientStop[]"/>. </returns>
        public static CanvasGradientStop[] LoadCanvasGradientStopArray(XElement element)
        {
            return
            (
                from stop
                in element.Elements()
                select new CanvasGradientStop
                {
                    Position = (float)stop.Attribute("Position"),
                    Color = FanKit.Transformers.XML.LoadColor(stop.Element("Color"))
                }
            ).ToArray();
        }

    }
}