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
        /// Saves the entire <see cref="Filter"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="filter"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveFilter(string elementName, Filter filter)
        {
            XElement element = new XElement(elementName);
            if (filter.Name != string.Empty) element.Add(new XAttribute("Name", filter.Name));

            element.Add(new XElement
            (
                "Adjustments",
                from adjustment
                in filter.Adjustments
                select XML.SaveIAdjustment("Adjustment", adjustment)
            ));

            return element;
        }

        /// <summary>
        ///  Loads a <see cref="Filter"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Filter"/>. </returns>
        public static Filter LoadFilter(XElement element)
        {
            Filter filter = new Filter();
            if (element.Attribute("Name") is XAttribute name) filter.Name = name.Value;

            if (element.Element("Adjustments") is XElement adjustments)
            {
                filter.Adjustments =
                (
                    from adjustment
                    in adjustments.Elements()
                    select XML.LoadIAdjustment(adjustment)
                ).ToList();
            }

            return filter;
        }

    }
}