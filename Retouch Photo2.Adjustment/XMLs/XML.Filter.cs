using System.Collections.Generic;
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
        ///  Loads a <see cref="Filter"/>s from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Filter"/>s. </returns>
        public static IEnumerable<Filter> LoadFilters(XDocument document)
        {
            XElement root = document.Element("Root");

            return
                from filter
                in root.Elements()
                select XML.LoadFilter(filter);
        }


        /// <summary>
        /// Saves the entire <see cref="Filter"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="filter"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveFilter(string elementName, Filter filter)
        {
            return new XElement
            (
                elementName,
                new XAttribute("Name", filter.Name),
                (
                    from a
                    in filter.Adjustments
                    select XML.SaveIAdjustment("Adjustment", a)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Filter"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Filter"/>. </returns>
        public static Filter LoadFilter(XElement element)
        {
            return new Filter
            {
                Name = element.Attribute("Name").Value,
                Adjustments =
                   from a
                   in element.Elements()
                   select XML.LoadIAdjustment(a)
            };
        }

    }
}