using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class XML
    {


        /// <summary>
        /// Load <see cref="Photo"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product photos. </returns>
        public static IEnumerable<Photo> LoadPhotosFile()
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/Photos.xml";

            try
            {
                XDocument document = XDocument.Load(path);

                IEnumerable<Photo> photos = Retouch_Photo2.Elements.XML.LoadPhotos(document);
                return photos;
            }
            catch (Exception)
            {
                IEnumerable<Photo> photos = new List<Photo>();
                return photos;
            }
        }


        public static async Task SavePhotoFile(StorageFolder zipFolder, IEnumerable<Photo> photos)
        {
            XDocument document = Retouch_Photo2.Elements.XML.SavePhotos(photos);

            //Save the project xml file.      
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
