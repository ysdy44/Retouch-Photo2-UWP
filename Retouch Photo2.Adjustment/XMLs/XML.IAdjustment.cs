// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Xml.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="IAdjustment"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="adjustment"> The destination <see cref="IAdjustment"/>. </param>
        public static XElement SaveIAdjustment(string elementName, IAdjustment adjustment)
        {
            XElement element = new XElement(elementName);
            element.Add(new XAttribute("Type", adjustment.Type));
            {
                // SaveWith
                adjustment.SaveWith(element);
            }
            return element;
        }

        /// <summary>
        ///  Loads a <see cref="IAdjustment"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IAdjustment"/>. </returns>
        public static IAdjustment LoadIAdjustment(XElement element)
        {
            // Load
            string type2 = element.Attribute("Type") is XAttribute type ? type.Value : null;
            IAdjustment adjustment = XML.CreateAdjustment(type2);
            {
                adjustment.Load(element);
            }
            return adjustment;
        }

    }
}