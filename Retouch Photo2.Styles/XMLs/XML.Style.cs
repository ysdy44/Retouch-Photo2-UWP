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
        /// Saves the entire <see cref="IStyle"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="style"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveStyle(string elementName, IStyle style)
        {
            XElement element = new XElement(elementName);

            if (style.Name != string.Empty) element.Add(new XAttribute("Name", style.Name));

            element.Add(new XElement("IsFollowTransform", style.IsFollowTransform));

            element.Add(new XElement("IsStrokeBehindFill", style.IsStrokeBehindFill));

            if (style.Fill != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Fill", style.Fill));
            if (style.Stroke != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Stroke", style.Stroke));
            
            element.Add(new XElement("IsStrokeWidthFollowScale", style.IsStrokeWidthFollowScale));

            element.Add(new XElement("StrokeWidth", style.StrokeWidth));
            if (style.StrokeStyle != null) element.Add(Retouch_Photo2.Strokes.XML.SaveStrokeStyle("StrokeStyle", style.StrokeStyle));
            if (style.Transparency != null) element.Add(Retouch_Photo2.Brushs.XML.SaveBrush("Transparency", style.Transparency));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="IStyle"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IStyle"/>. </returns>
        public static IStyle LoadStyle(XElement element)
        {
            IStyle style = new Style();

            if (element.Attribute("Name") is XAttribute name) style.Name = name.Value;

            if (element.Element("IsFollowTransform") is XElement isFollowTransform) style.IsFollowTransform = (bool)isFollowTransform;

            if (element.Element("IsStrokeBehindFill") is XElement isStrokeBehindFill) style.IsStrokeBehindFill = (bool)isStrokeBehindFill;
            
            if (element.Element("Fill") is XElement fill) style.Fill = Retouch_Photo2.Brushs.XML.LoadBrush(fill);
            if (element.Element("Stroke") is XElement stroke) style.Stroke = Retouch_Photo2.Brushs.XML.LoadBrush(stroke);

            if (element.Element("IsStrokeWidthFollowScale") is XElement isStrokeWidthFollowScale) style.IsStrokeWidthFollowScale = (bool)isStrokeWidthFollowScale;

            if (element.Element("StrokeWidth") is XElement strokeWidth) style.StrokeWidth = (float)strokeWidth;
            if (element.Element("StrokeStyle") is XElement strokeStyle) style.StrokeStyle = Retouch_Photo2.Strokes.XML.LoadStrokeStyle(strokeStyle);
            if (element.Element("Transparency") is XElement transparency) style.Transparency = Retouch_Photo2.Brushs.XML.LoadBrush(transparency);

            return style;
        }

    }
}