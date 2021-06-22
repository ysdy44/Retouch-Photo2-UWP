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
        /// Saves the entire <see cref="Filter"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="filter"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveFilter(string elementName, Filter filter)
        {
            XElement element = new XElement(elementName);
            if ((filter.Name is null) == false) element.Add(new XAttribute("Name", filter.Name));
            if ((filter.Strings is null) == false) element.Add(Retouch_Photo2.Elements.XML.SaveStrings("Strings", filter.Strings));
            element.Add
            (
                from adjustment
                in filter.Adjustments
                select Retouch_Photo2.Adjustments.XML.SaveIAdjustment("Adjustment", adjustment)
            );

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
            if (element.Element("Strings") is XElement strings) filter.Strings = Retouch_Photo2.Elements.XML.LoadStrings(strings);
            filter.Adjustments =
            (
                from adjustment
                in element.Elements("Adjustment")
                select Retouch_Photo2.Adjustments.XML.LoadIAdjustment(adjustment)
            ).ToList();

            return filter;
        }

    }
}