// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Retouch_Photo2.ViewModels;
using System;
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
        /// Construct <see cref="Setting"/>s File (Open from Defult, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="Setting"/>. </returns>
        public static async Task<Setting> ConstructSettingFile()
        {
            StorageFile file = null;
            bool isLocalSettingExists = await FileUtil.IsFileExistsInLocalFolder("Setting.xml");

            if (isLocalSettingExists)
            {
                //Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Setting.xml");
            }

            if (file != null)
            {
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XDocument document = XDocument.Load(stream);

                    Setting setting = Retouch_Photo2.ViewModels.XML.LoadSetting(document);
                    if (setting != null) return null;

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