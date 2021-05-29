// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Collections.Generic;
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
        /// Saves the entire <see cref="Filter"/>Categorys to a XDocument.
        /// </summary>
        /// <param name="filterCategorys"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveFilterCategorys(IEnumerable<FilterCategory> filterCategorys)
        {
            return new XDocument
            (
                // Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                     from filterCategory
                     in filterCategorys
                     select XML.SaveFilterCategory("FilterCategory", filterCategory)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Filter"/>Categorys from an XElement.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="Filter"/>s. </returns>
        public static IEnumerable<FilterCategory> LoadFilterCategorys(XDocument document)
        {
            if (document.Element("Root") is XElement root)
            {
                if (root.Elements("FilterCategory") is IEnumerable<XElement> filterCategorys)
                {
                    return
                    (
                        from filterCategory
                        in filterCategorys
                        select XML.LoadFilterCategory(filterCategory)
                    );
                }
            }

            return null;
        }
        
    }
}