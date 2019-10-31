using Retouch_Photo2.Blends;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="ImageStr"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="image"> The destination <see cref="ImageStr"/>. </param>
        public static XElement SaveImageStr(string elementName, ImageStr image)
        {
            return new XElement
            (
                elementName,
                new XElement("Name", image.Name),
                new XElement("FileType", image.FileType),
                new XElement("FolderRelativeId", image.FolderRelativeId)
            );
        }

        /// <summary>
        ///  Loads a <see cref="ImageStr"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="ImageStr"/>. </returns>
        public static ImageStr LoadImageStr(XElement element)
        {
            return new ImageStr
            {
                Name = element.Element("Name").Value,
                FileType = element.Element("FileType").Value,
                FolderRelativeId = element.Element("FolderRelativeId").Value,
            };
        }

    }
}