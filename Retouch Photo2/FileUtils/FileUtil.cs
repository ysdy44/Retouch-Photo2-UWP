using Retouch_Photo2.ViewModels;
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
        /// Rename zip file and thumbnail.
        /// </summary>
        /// <param name="item"> The old item.</param>
        /// <param name="newName"> The new name.</param>
        /// <returns> (New name, New zip File name, New thumbnail name). </returns>
        public static async Task RenameZipFileAndThumbnail(ProjectViewItem item, string newName)
        {
            //Rename zip file.
            StorageFile zipFile = await StorageFile.GetFileFromPathAsync(item.Photo2pkFilePath);
            await zipFile.RenameAsync($"{newName}.photo2pk");

            //Rename thumbnail image.
            StorageFile thumbnail = await StorageFile.GetFileFromPathAsync(item.ThumbnailPath);
            await thumbnail.RenameAsync($"{newName}.png");

            item.Rename(newName, zipFile.Path, thumbnail.Path);
        }

        /// <summary>
        /// Duplicate zip file and thumbnail.
        /// </summary>
        /// <param name="oldName"> The old name.</param>
        /// <param name="newName"> The new name.</param>
        public static async Task<StorageFile> DuplicateZipFileAndThumbnail(string oldName, string newName)
        {
            StorageFile zipFile = null;

            try
            {
                //Duplicate zip file.
                zipFile = await ApplicationData.Current.LocalFolder.GetFileAsync($"{oldName}.photo2pk");
                await zipFile.CopyAsync(ApplicationData.Current.LocalFolder, $"{newName}.photo2pk", NameCollisionOption.ReplaceExisting);
            }
            catch (Exception) { }

            try
            {
                //Duplicate thumbnail image.
                StorageFile thumbnail = await ApplicationData.Current.LocalFolder.GetFileAsync($"{oldName}.png");
                await thumbnail.CopyAsync(ApplicationData.Current.LocalFolder, $"{newName}.png", NameCollisionOption.ReplaceExisting);
            }
            catch (Exception) { }

            return zipFile;
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
                StorageFile zipFile = await ApplicationData.Current.LocalFolder.GetFileAsync($"{name}.photo2pk");
                if (zipFile != null) await zipFile.DeleteAsync();
            }
            catch (Exception) { }

            try
            {
                //Delete thumbnail image.
                StorageFile thumbnail = await ApplicationData.Current.LocalFolder.GetFileAsync($"{name}.png");
                if (thumbnail != null) await thumbnail.DeleteAsync();
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Delete all files in temp folder.
        /// </summary>
        public static async Task DeleteAllInTemporaryFolder()
        {
            IReadOnlyList<StorageFile> items = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            foreach (StorageFile item in items)
            {
                await item.DeleteAsync();
            }
        }

        /// <summary>
        /// Find all zip file in local folder.
        /// </summary>
        /// <returns> The all project file. </returns>
        public static async Task<IEnumerable<StorageFile>> FIndFilesInLocalFolder()
        {
            //get all file.
            IReadOnlyList<StorageFile> files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFile> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            //Ordered
            IEnumerable<StorageFile> orderedPhotos = from flie in orderedFiles where flie.FileType == ".photo2pk" select flie;
            return orderedPhotos;
        }

        
    }
}