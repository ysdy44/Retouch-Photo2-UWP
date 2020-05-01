using System.Xml.Linq;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="IBrush"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="brush"> The destination <see cref="IBrush"/>. </param>
        public static XElement SaveBrush(string elementName, IBrush brush)
        {
            XElement element = new XElement(elementName);
            element.Add(new XAttribute("Type", brush.Type));

            //SaveWith
            {
                brush.SaveWith(element);
            }
            return element;
        }

        /// <summary>
        ///  Loads a <see cref="IBrush"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IBrush"/>. </returns>
        public static IBrush LoadBrush(XElement element)
        {
            string type = element.Attribute("Type").Value;

            //Load
            IBrush brush = XML.CreateBrush(type);
            {
                brush.Load(element);
            }
            return brush;
        }

    }
}