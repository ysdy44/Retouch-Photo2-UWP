using Retouch_Photo2.Elements.MainPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {


        /// <summary>
        /// Find all zip folders in local folder.
        /// </summary>
        /// <returns> The all zip folders. </returns>
        public static async Task<IEnumerable<StorageFolder>> FIndZipFolders()
        {
            //get all folders.
            IReadOnlyList<StorageFolder> folders = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFolder> orderedFolders = folders.OrderByDescending(file => file.DateCreated);

            //Ordered
            IEnumerable<StorageFolder> zipFolders =
                from folder
                in orderedFolders
                where folder.Name.EndsWith(".photo2pk")
                select folder;

            return zipFolders;
        }
        


        #region ZipFolder


        /// <summary>
        /// Rename zip folder and thumbnail.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        public static async Task RenameZipFolder(string oldName, string newName, IProjectViewItem item)
        {
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            await zipFolder.RenameAsync($"{newName}.photo2pk");

            //Rename thumbnail.       
            string thumbnail = $"{zipFolder.Path}\\Thumbnail.png";
            item.Rename(newName, thumbnail);
        }


        /// <summary>
        /// Delete zip folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        public static async Task DeleteZipFolder(string name)
        {
            try
            {
                //Delete zip folder.
                StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");
                if (zipFolder != null) await zipFolder.DeleteAsync();
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Duplicate zip folder.
        /// </summary>
        /// <param name="oldName"> The old name.</param>
        /// <param name="newName"> The new name.</param>
        public static async Task<StorageFolder> DuplicateZipFolder(string oldName, string newName)
        {
            //Duplicate zip folder.
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            StorageFolder zipFolderNew = await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{newName}.photo2pk", CreationCollisionOption.ReplaceExisting);

            IReadOnlyList<StorageFile> files = await zipFolder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                await file.CopyAsync(zipFolderNew);
            }

            return zipFolderNew;
        }


        #endregion


        /// <summary>
        /// Move all files to temporary folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        /// <retrun> The exists. </retrun>
        public static async Task<bool> MoveAllAndReturn(string name)
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
                //Move to temporary folder
                await item.CopyAsync(ApplicationData.Current.TemporaryFolder);
            }
            return true;
        }

        /// <summary>
        /// Delete all files in zip folder and return zip folder.
        /// If not exists, create a new zip folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        public static async Task<StorageFolder> DeleteAllAndReturn(string name)
        {
            bool isZipFolderExists = await FileUtil.IsFileExistsInLocalFolder($"{name}.photo2pk");

            if (isZipFolderExists == false)
            {
                return await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{name}.photo2pk");
            }

            //Delete all in zip folder.
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");

            IReadOnlyList<StorageFile> items = await zipFolder.GetFilesAsync();
            foreach (StorageFile item in items)
            {
                await item.DeleteAsync();
            }

            return zipFolder;
        }


    }
}