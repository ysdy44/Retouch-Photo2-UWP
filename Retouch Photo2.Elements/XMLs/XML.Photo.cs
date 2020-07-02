using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Photo"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="photo"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SavePhoto(string elementName, Photo photo)
        {
            return new XElement
            (
                elementName,
                new XElement("Name", photo.Name),
                new XElement("FileType", photo.FileType),
                new XElement("FolderRelativeId", photo.FolderRelativeId)
            );
        }

        /// <summary>
        ///  Loads a <see cref="Photo"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Photo"/>. </returns>
        public static Photo LoadPhoto(XElement element)
        {
            Photo photo = new Photo();

            if (element.Element("Name") is XElement name) photo.Name = name.Value;
            if (element.Element("FileType") is XElement fileType) photo.FileType = fileType.Value;
            if (element.Element("FolderRelativeId") is XElement folderRelativeId) photo.FolderRelativeId = folderRelativeId.Value;

            return photo;
        }

    }
}