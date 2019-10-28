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
        /// Saves the entire <see cref="AdjustmentManager"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="adjustmentManager"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveAdjustmentManager(string elementName, AdjustmentManager adjustmentManager)
        {
            return new XElement
            (
                elementName,
                    from a
                    in adjustmentManager.Adjustments
                    select a.Save()
            );
        }

        /// <summary>
        ///  Loads a <see cref="AdjustmentManager"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="AdjustmentManager"/>. </returns>
        public static AdjustmentManager LoadAdjustmentManager(XElement element)
        {
            return new AdjustmentManager
            {
                Adjustments=
                (
                    from a
                    in element.Elements()
                    select XML.CreateAdjustment(a)
                ).ToList()
            };
        }

    }
}