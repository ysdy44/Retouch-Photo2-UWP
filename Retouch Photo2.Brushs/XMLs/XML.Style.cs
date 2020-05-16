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
                 XML.SaveBrush("FillBrush", style.FillBrush),
                 XML.SaveBrush("StrokeBrush", style.StrokeBrush),
                 new XElement("StrokeWidth", style.StrokeWidth)
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
            if (element.Element("FillBrush") is XElement fillBrush) style.FillBrush = XML.LoadBrush(fillBrush);
            if (element.Element("StrokeBrush") is XElement strokeBrush) style.StrokeBrush = XML.LoadBrush(strokeBrush);
            if (element.Element("StrokeWidth") is XElement strokeWidth) style.StrokeWidth = (float)strokeWidth;

            return style;
        }

    }
}