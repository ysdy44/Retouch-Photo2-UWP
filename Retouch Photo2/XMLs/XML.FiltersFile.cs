// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.Filters;
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
        /// Construct <see cref="FilterCategory"/>s File (Open from Application, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="FilterCategory"/>s. </returns>
        public static async Task<IEnumerable<FilterCategory>> ConstructFiltersFile()
        {
            StorageFile file = null;
            bool isLocalFilterExists = await FileUtil.IsFileExistsInLocalFolder("Filters.xml");

            if (isLocalFilterExists)
            {
                // Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Filters.xml");
            }
            else
            {
                // Read the file from the package.
                file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:/// XMLs/Filters.xml"));

                // Copy to the local folder.
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            if (file == null) return null;

            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                try
                {
                    XDocument document = XDocument.Load(stream);

                    IEnumerable<FilterCategory> source = Retouch_Photo2.Filters.XML.LoadFilterCategorys(document);
                    return source;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Save <see cref="FilterCategory"/> to local folder.
        /// </summary>
        /// <param name="filterCategorys"> The filter categorys. </param>
        public static async Task SaveFiltersFile(IEnumerable<FilterCategory> filterCategorys)
        {
            XDocument document = Retouch_Photo2.Filters.XML.SaveFilterCategorys(filterCategorys);

            // Save the Setting xml file.      
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Filters.xml", CreationCollisionOption.ReplaceExisting);
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