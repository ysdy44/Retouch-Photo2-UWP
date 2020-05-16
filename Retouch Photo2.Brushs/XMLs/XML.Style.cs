using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Style"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="style"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveStyle(string elementName, Style style)
        {
            return new XElement
            (
                 elementName,
                 new XElement("IsFollowTransform", style.IsFollowTransform),
                 XML.SaveBrush("Fill", style.Fill),
                 XML.SaveBrush("Stroke", style.Stroke),
                 new XElement("StrokeWidth", style.StrokeWidth),
                 Retouch_Photo2.Strokes.XML.SaveStrokeStyle("StrokeStyle", style.StrokeStyle)
            );
        }

        /// <summary>
        ///  Loads a <see cref="Style"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Style"/>. </returns>
        public static Style LoadStyle(XElement element)
        {
            Style style = new Style(); 

            if (element.Element("IsFollowTransform") is XElement isFollowTransform) style.IsFollowTransform = (bool)isFollowTransform;
            if (element.Element("Fill") is XElement fill) style.Fill = XML.LoadBrush(fill);
            if (element.Element("Stroke") is XElement stroke) style.Stroke = XML.LoadBrush(stroke);
            if (element.Element("StrokeWidth") is XElement strokeWidth) style.StrokeWidth = (float)strokeWidth;
            if (element.Element("StrokeStyle") is XElement strokeStyle) style.StrokeStyle = Retouch_Photo2.Strokes.XML.LoadStrokeStyle(strokeStyle);

            return style;
        }

    }
}