using Retouch_Photo2.Styles;
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
        /// Construct <see cref="StyleCategory"/>s File (Open from Application, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="StyleCategory"/>s. </returns>
        public static async Task<IEnumerable<StyleCategory>> ConstructStylesFile()
        {
            StorageFile file = null;
            bool isLocalStyleExists = await FileUtil.IsFileExistsInLocalFolder("Styles.xml");

            if (isLocalStyleExists)
            {
                //Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Styles.xml");
            }
            else
            {
                //Read the file from the package.
                file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///XMLs/Styles.xml"));

                //Copy to the local folder.
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XDocument document = XDocument.Load(stream);

                    IEnumerable<StyleCategory> source = Retouch_Photo2.Styles.XML.LoadStyleCategorys(document);

                    return source;
                }
            }
            return null;
        }

        /// <summary>
        /// Save <see cref="StyleCategory"/> to local folder.
        /// </summary>
        /// <param name="styleCategorys"> The Style categorys. </param>
        public static async Task SaveStylesFile(IEnumerable<StyleCategory> styleCategorys)
        {
            XDocument document = Retouch_Photo2.Styles.XML.SaveStyleCategorys(styleCategorys);

            //Save the Setting xml file.      
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Styles.xml", CreationCollisionOption.ReplaceExisting);
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