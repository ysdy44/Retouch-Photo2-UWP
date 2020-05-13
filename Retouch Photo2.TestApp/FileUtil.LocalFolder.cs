using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Elements.MainPages;
using Retouch_Photo2.ViewModels;
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
        /// <returns> The all project file. </returns>
        public static async Task<IEnumerable<StorageFolder>> FIndZipFolderInLocalFolder()
        {
            //get all folders.
            IReadOnlyList<StorageFolder> files = await ApplicationData.Current.LocalFolder.GetFoldersAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFolder> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            //Ordered
            IEnumerable<StorageFolder> orderedPhotos = from folder in orderedFiles where folder.Name.EndsWith(".photo2pk") select folder;
            return orderedPhotos;
        }


        #region ZipFile & Thumbnail


        /// <summary>
        /// Rename zip file and thumbnail.
        /// </summary>
        /// <param name="item"> The old item.</param>
        /// <param name="newName"> The new name.</param>
        /// <returns> (New name, New zip File name, New thumbnail name). </returns>
        public static async Task RenameZipFileAndThumbnail(ProjectViewItem item, string newName)
        {
            //Rename zip folder.
            StorageFolder zipFolder = await StorageFolder.GetFolderFromPathAsync(item.ZipFolderPath);
            await zipFolder.RenameAsync($"{newName}.photo2pk");

            StorageFile thumbnail = await zipFolder.GetFileAsync("Thumbnail.png");

            item.Rename(newName, zipFolder.Path, thumbnail.Path);
        }

        /// <summary>
        /// Duplicate zip file and thumbnail.
        /// </summary>
        /// <param name="oldName"> The old name.</param>
        /// <param name="newName"> The new name.</param>
        public static async Task<StorageFolder> DuplicateZipFileAndThumbnail(string oldName, string newName)
        {
            //Duplicate zip file.
            StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{oldName}.photo2pk");
            StorageFolder zipFolderNew = await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{newName}.photo2pk", CreationCollisionOption.ReplaceExisting);

            var files = await zipFolder.GetFilesAsync();
            foreach (var item in files)
            {
                await item.CopyAsync(zipFolderNew);
            }

            return zipFolderNew;
        }

        /// <summary>
        /// Delete zip file and thumbnail.
        /// </summary>
        /// <param name="name"> The name.</param>
        public static async Task DeleteZipFileAndThumbnail(string name)
        {
            try
            {
                //Delete zip file.
                StorageFolder zipFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");
                if (zipFolder != null) await zipFolder.DeleteAsync();
            }
            catch (Exception) { }
        }


        #endregion


        #region Thumbnail: Save


        public static async void SaveThumbnailAsync(CanvasRenderTarget thumbnail, string name)
        {
            bool isExits = await ApplicationLocalTextFileUtility.IsFileExistsInTemporaryFolder("Thumbnail.png");
            StorageFile file = null;

            if (isExits)
            {
                file = await ApplicationData.Current.TemporaryFolder.GetFileAsync("Thumbnail.png");
            }
            else
            {
                file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Thumbnail.png");
            }

            //File       
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await thumbnail.SaveAsync(fileStream, CanvasBitmapFileFormat.Png);
            }
        }


        #endregion

    }
}