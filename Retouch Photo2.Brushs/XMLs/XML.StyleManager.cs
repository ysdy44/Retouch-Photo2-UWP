using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="StyleManager"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="styleManager"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveStyleManager(string elementName, StyleManager styleManager)
        {
            return new XElement
            (
                 elementName,
                 XML.SaveBrush("FillBrush", styleManager.FillBrush),
                 XML.SaveBrush("StrokeBrush", styleManager.StrokeBrush),
                 new XElement("StrokeWidth", styleManager.StrokeWidth)
            );
        }

        /// <summary>
        ///  Loads a <see cref="StyleManager"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="StyleManager"/>. </returns>
        public static StyleManager LoadStyleManager(XElement element)
        {
            return new StyleManager
            {
                FillBrush = XML.LoadBrush(element.Element("FillBrush")),
                StrokeBrush = XML.LoadBrush(element.Element("StrokeBrush")),
                StrokeWidth = (float)element.Element("StrokeWidth"),
            };
        }

    }
}