using System.Xml.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Photocopier"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="photocopier"> The destination <see cref="Photocopier"/>. </param>
        public static XElement SavePhotocopier(string elementName, Photocopier photocopier)
        {
            return new XElement
            (
                elementName,
                new XElement("Name", photocopier.Name),
                new XElement("FileType", photocopier.FileType),
                new XElement("FolderRelativeId", photocopier.FolderRelativeId)
            );
        }

        /// <summary>
        ///  Loads a <see cref="Photocopier"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Photocopier"/>. </returns>
        public static Photocopier LoadPhotocopier(XElement element)
        {
            return new Photocopier
            {
                Name = element.Element("Name").Value,
                FileType = element.Element("FileType").Value,
                FolderRelativeId = element.Element("FolderRelativeId").Value,
            };
        }

    }
}