using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
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
        /// Construct <see cref="Setting"/>s File (Open from Defult, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="Setting"/>. </returns>
        public static async Task<Setting> ConstructSettingFile()
        {
            StorageFile file = null;
            bool isLocalSettingExists = await ApplicationLocalTextFileUtility.IsFileExistsInLocalFolder("Setting.xml");

            if (isLocalSettingExists)
            {
                //Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Setting.xml");
            }
            else
            {
                Setting setting = new Setting();

                //Save to the local folder.
                await FileUtil.SaveSettingFile(setting);

                return setting;
            }

            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XDocument document = XDocument.Load(stream);

                    Setting setting = Retouch_Photo2.ViewModels.XML.LoadSetting(document);
                    return setting;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Save <see cref="Setting"/> to local folder.
        /// </summary>
        /// <param name="setting"> The Setting. </param>
        public static async Task SaveSettingFile(Setting setting)
        {
            XDocument document = Retouch_Photo2.ViewModels.XML.SaveSetting(setting);

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