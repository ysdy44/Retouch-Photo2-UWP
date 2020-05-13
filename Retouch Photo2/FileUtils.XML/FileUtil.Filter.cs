using Retouch_Photo2.Adjustments;
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
    public static partial class FileUtil
    {

        /// <summary>
        /// Construct <see cref="Filter"/>s File (Open from Application, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="Filter"/>s. </returns>
        public static async Task<IEnumerable<Filter>> ConstructFilterFile()
        {
            StorageFile file = null;
            bool isLocalFilterExists = await ApplicationLocalTextFileUtility.IsFileExistsInLocalFolder("Filter.xml");

            if (isLocalFilterExists)
            {
                //Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Filter.xml");
            }
            else
            {
                //Read the file from the package.
                file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils.XML/Filter.xml"));

                //Copy to the local folder.
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XDocument document = XDocument.Load(stream);

                    IEnumerable<Filter> source = Retouch_Photo2.Adjustments.XML.LoadFilters(document);
                    return source;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Save <see cref="Filter"/> to local folder.
        /// </summary>
        /// <param name="filters"> The Filters. </param>
        public static async Task SaveFilterFile(IEnumerable<Filter> filters)
        {
            XDocument document = Retouch_Photo2.Adjustments.XML.SaveFilters(filters);

            //Save the Setting xml file.      
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Setting.xml", CreationCollisionOption.ReplaceExisting);
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
