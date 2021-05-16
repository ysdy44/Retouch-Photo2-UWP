// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire strings to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="strings"> The destination strings. </param>
        public static XElement SaveStrings(string elementName, IDictionary<string, string> strings)
        {
            return new XElement(elementName,
            (
                from item
                in strings
                select new XElement("String", new XAttribute("Key", item.Key), new XAttribute("Value", item.Value))
            ));
        }

        /// <summary>
        ///  Loads a strings from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded strings. </returns>
        public static IDictionary<string, string> LoadStrings(XElement element)
        {          
            return element.Elements("String").ToDictionary
            (
                item => item.Attribute("Key").Value,
                item => item.Attribute("Value").Value
            );
        }

    }
}