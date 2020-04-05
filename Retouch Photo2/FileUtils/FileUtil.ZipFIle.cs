using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {
        
        /// <summary>
        ///  Extract zip file from local folder to temporary folder.
        /// </summary>
        /// <param name="zipFilePath"> The path of zip file. </param>
        /// <returns> The extract project. </returns>
        public static async Task ExtractZipFile(string zipFilePath)
        {
            //Read the file stream
            StorageFile file = await StorageFile.GetFileFromPathAsync(zipFilePath);
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                //Unzip to temporary folder
                ZipArchive archive = new ZipArchive(stream);
                archive.ExtractToDirectory(ApplicationData.Current.TemporaryFolder.Path);
            }
        }
        
        /// <summary>
        /// Create a zip file from temporary folder to local folder.
        /// </summary>
        /// <param name="name"> The zip file name. </param>
        public static async Task CreateZipFile(string name)
        {
            //Delete if it exists in local folder.
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync($"{name}.photo2pk");
                if (file != null) await file.DeleteAsync();
            }
            catch (Exception) { }

            //Zip all file in temporary folder to local folder.
            string path = $"{ApplicationData.Current.LocalFolder.Path}\\{name}.photo2pk";
            ZipFile.CreateFromDirectory(ApplicationData.Current.TemporaryFolder.Path, path);
        }

        
    }
}