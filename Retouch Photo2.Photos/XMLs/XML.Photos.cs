using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Retouch_Photo2.Photos
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
                    select XML.SavePhoto("Photo", photo)
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
            if (document.Element("Root") is XElement root)
            {
                if (root.Elements("Photo") is IEnumerable<XElement> photos)
                {
                    return
                        from photo
                        in photos
                        select XML.LoadPhoto(photo);
                }
            }

            return null;
        }

    }
}