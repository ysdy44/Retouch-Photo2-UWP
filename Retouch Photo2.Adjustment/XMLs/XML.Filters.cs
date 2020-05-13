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
        /// Saves the entire <see cref="Filter"/>s to a XDocument.
        /// </summary>
        /// <param name="filters"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveFilters(IEnumerable<Filter> filters)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                     from filter
                     in filters
                     select XML.SaveFilter("Layer", filter)
                )
            );
        }

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

    }
}