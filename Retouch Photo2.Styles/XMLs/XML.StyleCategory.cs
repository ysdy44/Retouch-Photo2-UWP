// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="StyleCategory"/> to a XDocument.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="StyleCategory"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XElement SaveStyleCategory(string elementName, StyleCategory StyleCategory)
        {
            XElement element = new XElement(elementName);
            element.Add(new XAttribute("Name", StyleCategory.Name));

            element.Add
            (
                from Style
                in StyleCategory.Styles
                select XML.SaveStyle("Style", Style)
            );

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="StyleCategory"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IStyle"/>s. </returns>
        public static StyleCategory LoadStyleCategory(XElement element)
        {
            StyleCategory StyleCategory = new StyleCategory();
            if (element.Attribute("Name") is XAttribute name) StyleCategory.Name = name.Value;

            StyleCategory.Styles =
            (
                from Style
                in element.Elements("Style")
                select XML.LoadStyle(Style)
            ).ToList();

            return StyleCategory;
        }

    }
}
