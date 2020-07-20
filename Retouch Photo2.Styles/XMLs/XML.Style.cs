using System;
using System.Xml.Linq;
using Retouch_Photo2.Brushs;

namespace Retouch_Photo2.Styles
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
            XElement element = new XElement(elementName);

            if (style.Name != string.Empty) element.Add(new XAttribute("Name", style.Name));

            element.Add(new XElement("IsFollowTransform", style.IsFollowTransform));

            if (style.Fill != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Fill", style.Fill));
            if (style.Stroke != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Stroke", style.Stroke));
            element.Add(new XElement("StrokeWidth", style.StrokeWidth));
            if (style.StrokeStyle != null) element.Add(Retouch_Photo2.Strokes.XML.SaveStrokeStyle("StrokeStyle", style.StrokeStyle));
            if (style.Transparency != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Transparency", style.Transparency));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="Style"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Style"/>. </returns>
        public static Style LoadStyle(XElement element)
        {
            Style style = new Style();

            if (element.Attribute("Name") is XAttribute name) style.Name = name.Value;

            if (element.Element("IsFollowTransform") is XElement isFollowTransform) style.IsFollowTransform = (bool)isFollowTransform;

            if (element.Element("Fill") is XElement fill) style.Fill = Retouch_Photo2.Brushs.XML.LoadBrush(fill);
            if (element.Element("Stroke") is XElement stroke) style.Stroke = Retouch_Photo2.Brushs.XML.LoadBrush(stroke);
            if (element.Element("StrokeWidth") is XElement strokeWidth) style.StrokeWidth = (float)strokeWidth;
            if (element.Element("StrokeStyle") is XElement strokeStyle) style.StrokeStyle = Retouch_Photo2.Strokes.XML.LoadStrokeStyle(strokeStyle);
            if (element.Element("Transparency") is XElement transparency) style.Transparency = Retouch_Photo2.Brushs.XML.LoadBrush(transparency);

            return style;
        }

    }
}