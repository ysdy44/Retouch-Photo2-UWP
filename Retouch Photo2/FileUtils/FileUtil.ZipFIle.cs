using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Delete all files in temp folder.
        /// </summary>
        public static async Task DeleteCacheAsync()
        {
            IReadOnlyList<StorageFile> items = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            foreach (StorageFile item in items)
            {
                await item.DeleteAsync();
            }
        }


        /// <summary>
        /// Load project and extract zip file to temp folder.
        /// </summary>
        /// <param name="zipFilePath"> The path of zip file. </param>
        /// <returns> The extract project. </returns>
        public static async Task ExtractToDirectory(string zipFilePath)
        {
            //Read the file stream
            StorageFile file = await StorageFile.GetFileFromPathAsync(zipFilePath);
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                //Unzip to temporary folder
                ZipArchive archive = new ZipArchive(stream);
                archive.ExtractToDirectory(ApplicationData.Current.TemporaryFolder.Path, true);
            }
        }

        /// <summary>
        /// Create a zip file from temporary folder to local folder.
        /// </summary>
        /// <param name="name"> The zip file name. </param>
        public static async Task CreateFromDirectory(string name)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync($"{name}.photo2pk");
                if (file != null) await file.DeleteAsync();
            }
            catch (Exception) { }

            string path = $"{ApplicationData.Current.LocalFolder.Path}/{name}.photo2pk";
            ZipFile.CreateFromDirectory(ApplicationData.Current.TemporaryFolder.Path, path);
        }

    }
}