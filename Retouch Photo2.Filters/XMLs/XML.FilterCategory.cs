// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Filters
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="FilterCategory"/> to a XDocument.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="filterCategory"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        private static XElement SaveFilterCategory(string elementName, FilterCategory filterCategory)
        {
            XElement element = new XElement(elementName);
            if ((filterCategory.Name is null) == false) element.Add(new XAttribute("Name", filterCategory.Name));
            if ((filterCategory.Strings is null) == false) element.Add(Retouch_Photo2.Elements.XML.SaveStrings("Strings", filterCategory.Strings));
            element.Add
            (
                from filter
                in filterCategory.Filters
                select XML.SaveFilter("Filter", filter)
            );

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="FilterCategory"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Filter"/>s. </returns>
        private static FilterCategory LoadFilterCategory(XElement element)
        {
            FilterCategory filterCategory = new FilterCategory();
            if (element.Attribute("Name") is XAttribute name) filterCategory.Name = name.Value;
            if (element.Element("Strings") is XElement strings) filterCategory.Strings = Retouch_Photo2.Elements.XML.LoadStrings(strings);
            filterCategory.Filters =
            (
                from filter
                in element.Elements("Filter")
                select XML.LoadFilter(filter)
            ).ToList();

            return filterCategory;
        }

    }
}
