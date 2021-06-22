using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

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

            // Sort by Time
            IOrderedEnumerable<StorageFolder> orderedFolders = folders.OrderByDescending(file => file.DateCreated);

            // Ordered
            IEnumerable<StorageFolder> zipFolders =
                from zipFolder
                in orderedFolders
                where zipFolder.Name.EndsWith(".photo2pk")
                select zipFolder;

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
            StorageFile file = await StorageFile.GetFileFromPathAsync(url);

            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                return new ProjectViewItem
                {
                    Name = name,
                    ImageSource = await FileUtil.GetImageSource(stream)
                };
            }
        }

        /// <summary>
        ///Rename zip folder and thumbnail.
        /// </summary>
        /// <param name="oldName"> The old name. </param>
        /// <param name="newName"> The new name. </param>
        public static async Task RenameZipFolder(string oldName, string newName)
        {
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            await zipFolder.RenameAsync($"{newName}.photo2pk");
        }


        /// <summary>
        /// Delete zip folder.
        /// </summary>
        /// <param name="name"> The zip folder name. </param>
        /// <returns> Deleted successful? </returns>
        public static async Task<bool> DeleteZipFolder(string name)
        {
            try
            {
                // Delete zip folder.
                StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");
                if (zipFolder is null) return false;

                await zipFolder.DeleteAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Duplicate zip folder.
        /// </summary>
        /// <param name="oldName"> The old name.</param>
        /// <param name="newName"> The new name.</param>
        /// <returns> The product ProjectViewItem. </returns>
        public static async Task<IProjectViewItem> DuplicateZipFolder(string oldName, string newName)
        {
            ProjectViewItem item = new ProjectViewItem
            {
                Name = newName
            };

            // Duplicate zip folder.
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            StorageFolder zipFolderNew = await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{newName}.photo2pk", CreationCollisionOption.ReplaceExisting);

            IReadOnlyList<StorageFile> files = await zipFolder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                await file.CopyAsync(zipFolderNew);

                if (file.Name == "Thumbnail.png")
                {
                    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        item.ImageSource = await FileUtil.GetImageSource(stream);
                    }
                }
            }

            return item;
        }


    }
}