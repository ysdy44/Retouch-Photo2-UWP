using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Find all zip folders in local folder.
        /// </summary>
        /// <returns> The all zip folders. </returns>
        public static async Task<IEnumerable<StorageFolder>> FIndAllZipFolders()
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


        /// <summary>
        /// Initializes ProjectViewItem by zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder. </param>
        /// <returns> The product ProjectViewItem. </returns>
        public static async Task<IProjectViewItem> ConstructProjectViewItem(StorageFolder zipFolder)
        {
            string name = zipFolder.DisplayName.Replace(".photo2pk", "");
            string url = $"{zipFolder.Path}\\Thumbnail.png";
            WriteableBitmap imageSource = await FileUtil.DisplayThumbnailFile(url); 

            return new ProjectViewItem
            {
                Name = name,
                ImageSource = imageSource
            };
        }


        /// <summary>
        /// Rename zip folder and thumbnail.
        /// </summary>
        /// <param name="oldName"> The old name. </param>
        /// <param name="newName"> The new name. </param>
        /// <param name="item"> The IProjectViewItem. </param>
        public static async Task RenameZipFolder(string oldName, string newName, IProjectViewItem item)
        {
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            await zipFolder.RenameAsync($"{newName}.photo2pk");
            
            string url = $"{zipFolder.Path}\\Thumbnail.png";
            WriteableBitmap imageSource = await FileUtil.DisplayThumbnailFile(url);

            item.Name = newName;
            item.ImageSource = imageSource;
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


    }
}