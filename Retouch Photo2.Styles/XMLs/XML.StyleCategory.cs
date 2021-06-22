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
        /// <param name="styleCategory"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        private static XElement SaveStyleCategory(string elementName, StyleCategory styleCategory)
        {
            XElement element = new XElement(elementName);
            if ((styleCategory.Name is null) == false) element.Add(new XAttribute("Name", styleCategory.Name));
            if ((styleCategory.Strings is null) == false) element.Add(Retouch_Photo2.Elements.XML.SaveStrings("Strings", styleCategory.Strings));
            element.Add
            (
                from style
                in styleCategory.Styles
                select XML.SaveStyle("Style", style)
            );

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="StyleCategory"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="IStyle"/>s. </returns>
        private static StyleCategory LoadStyleCategory(XElement element)
        {
            StyleCategory styleCategory = new StyleCategory();
            if (element.Attribute("Name") is XAttribute name) styleCategory.Name = name.Value;
            if (element.Element("Strings") is XElement strings) styleCategory.Strings = Retouch_Photo2.Elements.XML.LoadStrings(strings);
            styleCategory.Styles =
            (
                from style
                in element.Elements("Style")
                select XML.LoadStyle(style)
            ).ToList();

            return styleCategory;
        }

    }
}
