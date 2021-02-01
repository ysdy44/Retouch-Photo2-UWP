// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
public static partial class XML
    {
        
        /// <summary>
        /// Create a <see cref="DeviceLayoutType"/> from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="DeviceLayoutType"/>. </returns>
        public static DeviceLayoutType CreateDeviceLayoutType(string type)
        {
            switch (type)
            {
                case "Phone": return DeviceLayoutType.Phone;
                case "Pad": return DeviceLayoutType.Pad;
                case "PC": return DeviceLayoutType.PC;

                default: return DeviceLayoutType.PC;
            }
        }

    }
}