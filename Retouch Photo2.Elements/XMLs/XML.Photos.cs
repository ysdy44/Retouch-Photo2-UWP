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
        /// Saves the entire <see cref="Photo"/>s to a XDocument.
        /// </summary>
        /// <param name="photos"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SavePhotos(IEnumerable<Photo> photos)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    from photo
                    in photos
                    select new XElement
                    (
                         "Photos",
                         new XElement("Name", photo.Name),
                         new XElement("FileType", photo.FileType),
                         new XElement("FolderRelativeId", photo.FolderRelativeId)
                    )
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="Photo"/>s from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="Photo"/>s. </returns>
        public static IEnumerable<Photo> LoadPhotos(XDocument document)
        {
            XElement root = document.Element("Root");

            return 
                from photo
                in root.Elements()
                select new Photo
                {
                    Name = photo.Element("Name").Value,
                    FileType = photo.Element("FileType").Value,
                    FolderRelativeId = photo.Element("FolderRelativeId").Value,
                };
        }

    }
}