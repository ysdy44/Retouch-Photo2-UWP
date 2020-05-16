using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="FilterCategory"/> to a XDocument.
        /// </summary>
        /// <param name="filterCategory"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XElement SaveFilterCategory(string elementName, FilterCategory filterCategory)
        {
            XElement element = new XElement(elementName);
            element.Add(new XAttribute("Name", filterCategory.Name));

            element.Add(new XElement
            (
                "Filters",
                from filter
                in filterCategory.Filters
                select XML.SaveFilter("Filter", filter)
            ));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="FilterCategory"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Filter"/>s. </returns>
        public static FilterCategory LoadFilterCategory(XElement element)
        {
            FilterCategory filterCategory = new FilterCategory();
            if (element.Attribute("Name") is XAttribute name) filterCategory.Name = name.Value;
            
            if (element.Element("Filters") is XElement filters)
            {
                filterCategory.Filters =
                (
                    from filter
                    in filters.Elements()
                    select XML.LoadFilter(filter)
                ).ToList();
            }

            return filterCategory;
        }

    }
}
