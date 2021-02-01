// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Xml.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {
        
        /// <summary>
        /// Saves the entire <see cref="DeviceLayout"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="deviceLayout"> The destination <see cref="DeviceLayout"/>. </param>
        public static XElement SaveDeviceLayout(string elementName, DeviceLayout deviceLayout)
        {
            return new XElement
            (
                elementName,
                new XAttribute("IsAdaptive", deviceLayout.IsAdaptive),
                new XAttribute("PhoneMaxWidth", deviceLayout.PhoneMaxWidth),
                new XAttribute("PadMaxWidth", deviceLayout.PadMaxWidth),
                new XAttribute("FallBackType", deviceLayout.FallBackType)
            );
        }
       
        /// <summary>
        ///  Loads a <see cref="DeviceLayout"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="DeviceLayout"/>. </returns>
        public static DeviceLayout LoadDeviceLayout(XElement element)
        {
            DeviceLayout deviceLayout = new DeviceLayout();

            if (element.Attribute("IsAdaptive") is XAttribute isAdaptive) deviceLayout.IsAdaptive = (bool)isAdaptive;
            if (element.Attribute("PhoneMaxWidth") is XAttribute phoneMaxWidth) deviceLayout.PhoneMaxWidth = (int)phoneMaxWidth;
            if (element.Attribute("PadMaxWidth") is XAttribute padMaxWidth) deviceLayout.PadMaxWidth = (int)padMaxWidth;
            if (element.Attribute("FallBackType") is XAttribute fallBackType) deviceLayout.FallBackType = XML.CreateDeviceLayoutType(fallBackType.Value);
            
            return deviceLayout;
        }

    }
}