using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provides constant and static methods 
    /// for reading and writing to local or package file.
    /// </summary>
    public static class ApplicationLocalTextFileUtility
    {

        /// <summary> 
        /// Read file from application package. 
        /// </summary> 
        /// <param name="fileName"></param>
        /// <returns> The default text. </returns>
        public static async Task<string> ReadFromApplicationPackage(string fileName)
        {
            Uri uri = new Uri(fileName);

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Read file from Local Folder. 
        /// </summary> 
        /// <param name="fileName"> The source file name. </param>
        /// <returns> The default text. </returns>
        public static async Task<string> ReadFromLocalFolder(string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The source file name. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExistsInLocalFolder(string fileName)
        {
            IStorageItem item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(fileName);
            return item != null;
        }

        /// <summary>
        /// Write file to Local Folder. 
        /// </summary> 
        /// <param name="text"> The source text. </param>
        /// <param name="fileName"> The source file name. </param>
        /// <returns> Return **true** if the file was saved successfully, otherwise **false**. </returns>
        public static async Task<bool> WriteToLocalFolder(string text, string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}