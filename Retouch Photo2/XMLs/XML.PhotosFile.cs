// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Photos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Load <see cref="Photo"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product <see cref="Photo"/>s. </returns>
        public static IEnumerable<Photo> LoadPhotosFile()
        {
            // Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/Photos.xml";

            try
            {
                XDocument document = XDocument.Load(path);

                IEnumerable<Photo> photos = Retouch_Photo2.Photos.XML.LoadPhotos(document);
                return photos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Save <see cref="Photo"/>s to zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder. </param>
        /// <param name="photos"> The photos. </param>
        public static async Task SavePhotosFile(StorageFolder zipFolder, IEnumerable<Photo> photos)
        {
            XDocument document = Retouch_Photo2.Photos.XML.SavePhotos(photos);

            // Save the xml file.      
            StorageFile file = await zipFolder.CreateFileAsync("Photos.xml", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (Stream stream = fileStream.AsStream())
                {
                    document.Save(stream);
                }
            }
        }


    }
}
