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
        /// Saves the entire <see cref="ImageRe"/>s to a XDocument.
        /// </summary>
        /// <param name="project"> The source data. </param>
        /// <returns> The saved XDocument. </returns>
        public static XDocument SaveImageRes(IEnumerable<ImageRe> imageRes)
        {
            return new XDocument
            (
                //Set the document definition for xml.
                new XDeclaration("1.0", "utf-8", "no"),
                new XElement
                (
                    "Root",
                    from imageRe
                    in imageRes
                    select new XElement
                    (
                         "ImageRe",
                         new XElement("Name", imageRe.Name),
                         new XElement("FileType", imageRe.FileType),
                         new XElement("FolderRelativeId", imageRe.FolderRelativeId)
                    )
                )
            );
        }

        /// <summary>
        ///  Loads a <see cref="ImageRe"/>s from an XDocument.
        /// </summary>
        /// <param name="document"> The source XDocument. </param>
        /// <returns> The loaded <see cref="ImageRe"/>s. </returns>
        public static IEnumerable<ImageRe> LoadImageRes(XDocument document)
        {
            XElement root = document.Element("Root");

            return 
                from imageRe
                in root.Elements()
                select new ImageRe
                {
                    Name = imageRe.Element("Name").Value,
                    FileType = imageRe.Element("FileType").Value,
                    FolderRelativeId = imageRe.Element("FolderRelativeId").Value,
                };
        }

    }
}