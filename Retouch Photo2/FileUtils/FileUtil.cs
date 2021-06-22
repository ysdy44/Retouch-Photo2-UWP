// Core:              ★★★
// Referenced:   ★★★
// Difficult:         ★★
// Only:              ★★★★★
// Complete:      ★★★
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2
{
    /// <summary>
    /// A collection of file-processing util methods.
    /// </summary>
    public static partial class FileUtil
    {

        /// <summary>
        /// Delet all files in temporary folder.
        /// </summary>
        public static async Task DeleteAllInTemporaryFolder()
        {
            try
            {
                IReadOnlyList<StorageFile> items = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
                foreach (StorageFile item in items)
                {
                    await item.DeleteAsync();
                }
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Move all files in local golder to temporary folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        /// <retrun> The exists. </retrun>
        public static async Task<bool> MoveAllInZipFolderToTemporaryFolder(string name)
        {
            try
            {
                StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");
                {
                    bool isZipFolderPhotosFileExists = await FileUtil.IsFileExists($"Photos.xml", zipFolder);
                    if (isZipFolderPhotosFileExists == false) return false;

                    bool isZipFolderProjectFileExists = await FileUtil.IsFileExists($"Project.xml", zipFolder);
                    if (isZipFolderProjectFileExists == false) return false;
                }


                IReadOnlyList<StorageFile> files = await zipFolder.GetFilesAsync();
                foreach (StorageFile item in files)
                {
                    // Move to temporary folder
                    await item.CopyAsync(ApplicationData.Current.TemporaryFolder);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Delete all files in zip folder and return zip folder.
        /// If not exists, create a new zip folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        public static async Task<StorageFolder> DeleteAllInZipFolder(string name)
        {
            bool isZipFolderExists = await FileUtil.IsFileExistsInLocalFolder($"{name}.photo2pk");

            if (isZipFolderExists == false)
            {
                return await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{name}.photo2pk");
            }

            // Delete all in zip folder.
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");

            IReadOnlyList<StorageFile> items = await zipFolder.GetFilesAsync();
            foreach (StorageFile item in items)
            {
                await item.DeleteAsync();
            }

            return zipFolder;
        }



        #region  IsExists


        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExistsInLocalFolder(string fileName) => await FileUtil.IsFileExists(fileName, ApplicationData.Current.LocalFolder);

        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExistsInTemporaryFolder(string fileName) => await FileUtil.IsFileExists(fileName, ApplicationData.Current.TemporaryFolder);

        /// <summary>
        /// To know if a file exists.
        /// </summary>
        /// <param name="fileName"> The file name. </param>
        /// <param name="folder"> The folder. </param>
        /// <returns> The exists. </returns>
        public static async Task<bool> IsFileExists(string fileName, StorageFolder folder)
        {
            IStorageItem item = await folder.TryGetItemAsync(fileName);
            return (item is null) == false;
        }


        #endregion


    }
}