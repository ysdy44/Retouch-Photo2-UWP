// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Collections.Generic;
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
        /// Saves the entire <see cref="IStyle"/>Categorys to a XDocument.
        /// </summary>
        /// <param name="styleCategorys"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveStyleCategorys(IEnumerable<StyleCategory> styleCategorys)
        {
            return new XDocument
            (
                // Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                     from style
                     in styleCategorys
                     select XML.SaveStyleCategory("StyleCategory", style)
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="IStyle"/>s from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="IStyle"/>s. </returns>
        /// <summary>
        public static IEnumerable<StyleCategory> LoadStyleCategorys(XDocument document)
        {
            if (document.Element("Root") is XElement root)
            {
                if (root.Elements("StyleCategory") is IEnumerable<XElement> styleCategorys)
                {
                    return
                    (
                        from styleCategory
                        in styleCategorys
                        select XML.LoadStyleCategory(styleCategory)
                    );
                }
            }

            return null;
        }
        
    }
}