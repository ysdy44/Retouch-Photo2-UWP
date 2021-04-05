// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System;
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
                new XAttribute("DashStyle", strokeStyle.DashStyle),
                new XAttribute("DashCap", strokeStyle.DashCap),
                new XAttribute("StartCap", strokeStyle.StartCap),
                new XAttribute("EndCap", strokeStyle.EndCap),
                new XAttribute("DashOffset", strokeStyle.DashOffset),

                new XAttribute("MiterLimit", strokeStyle.MiterLimit),
                new XAttribute("LineJoin", strokeStyle.LineJoin)
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

            if (element.Attribute("DashStyle") is XAttribute dash) 
            {
                try
                {
                    strokeStyle.DashStyle = (CanvasDashStyle)Enum.Parse(typeof(CanvasDashStyle), dash.Value);
                }
                catch (Exception) { }
            }
            if (element.Attribute("DashCap") is XAttribute cap) 
            {
                try
                {
                    strokeStyle.DashCap = (CanvasCapStyle)Enum.Parse(typeof(CanvasCapStyle), cap.Value);
                }
                catch (Exception) { }
            }
            if (element.Attribute("StartCap") is XAttribute startCap) 
            {
                try
                {
                    strokeStyle.StartCap = (CanvasCapStyle)Enum.Parse(typeof(CanvasCapStyle), startCap.Value);
                }
                catch (Exception) { }
            }
            if (element.Attribute("EndCap") is XAttribute endCap)
            {
                try
                {
                    strokeStyle.EndCap = (CanvasCapStyle)Enum.Parse(typeof(CanvasCapStyle), endCap.Value);
                }
                catch (Exception) { }
            }
            if (element.Attribute("DashOffset") is XAttribute offset) strokeStyle.DashOffset = (float)offset;

            if (element.Attribute("MiterLimit") is XAttribute miter) strokeStyle.MiterLimit = (float)miter;
            if (element.Attribute("LineJoin") is XAttribute join)
            {
                try
                {
                    strokeStyle.LineJoin = (CanvasLineJoin)Enum.Parse(typeof(CanvasLineJoin), join.Value);
                }
                catch (Exception) { }
            }

            return strokeStyle;
        }

    }
}