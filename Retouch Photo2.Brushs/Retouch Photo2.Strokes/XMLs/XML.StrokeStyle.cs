using System.Xml.Linq;
using Microsoft.Graphics.Canvas.Geometry;

namespace Retouch_Photo2.Strokes
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="CanvasStrokeStyle"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="strokeStyle"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveStrokeStyle(string elementName, CanvasStrokeStyle strokeStyle)
        {
            XElement element= new XElement
            (
                elementName,
                new XElement("DashStyle", strokeStyle.DashStyle),
                new XElement("DashCap", strokeStyle.DashCap),
                new XElement("StartCap", strokeStyle.StartCap),
                new XElement("EndCap", strokeStyle.EndCap),
                new XElement("DashOffset", strokeStyle.DashOffset),

                new XElement("MiterLimit", strokeStyle.MiterLimit),
                new XElement("LineJoin", strokeStyle.LineJoin)
            );

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="CanvasStrokeStyle"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="CanvasStrokeStyle"/>. </returns>
        public static CanvasStrokeStyle LoadStrokeStyle(XElement element)
        {
            CanvasStrokeStyle strokeStyle = new CanvasStrokeStyle();

            if (element.Element("DashStyle") is XElement dash) strokeStyle.DashStyle = XML.CreateDash(dash.Value);
            if (element.Element("DashCap") is XElement cap) strokeStyle.DashCap = XML.CreateCap(cap.Value);
            if (element.Element("StartCap") is XElement startCap) strokeStyle.StartCap = XML.CreateCap(startCap.Value);
            if (element.Element("EndCap") is XElement endCap) strokeStyle.EndCap = XML.CreateCap(endCap.Value);
            if (element.Element("DashOffset") is XElement offset) strokeStyle.DashOffset = (float)offset;

            if (element.Element("MiterLimit") is XElement miter) strokeStyle.MiterLimit = (float)miter;
            if (element.Element("LineJoin") is XElement join) strokeStyle.LineJoin = XML.CreateJoin(join.Value);

            return strokeStyle;
        }

    }
}