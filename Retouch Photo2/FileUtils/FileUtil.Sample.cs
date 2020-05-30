using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {
        /// <summary>
        /// Construct Samples File (Open from Application, Save from LocalFolder)
        /// </summary>
        public static async Task ConstructSampleFile()
        {
            //Read the file from the package.
            StorageFile file0 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/App.photo2pk.zip"));
            StorageFile file1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/Banner.photo2pk.zip"));
            StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/MFY.photo2pk.zip"));

            //Unzip to LocalFolder
            await FileUtil.ExtractToDirectory(file0);
            await FileUtil.ExtractToDirectory(file1);
            await FileUtil.ExtractToDirectory(file2);
        }

        /// <summary>
        /// Load project and extract zip to LocalFolder.
        /// </summary>
        /// <param name="zipFilePath"> The path of zip file. </param>
        /// <returns> The extract project. </returns>
        public static async Task ExtractToDirectory(StorageFile file)
        {
            //Read the file stream
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                //Unzip to LocalFolder
                ZipArchive archive = new ZipArchive(stream);
                archive.ExtractToDirectory(ApplicationData.Current.LocalFolder.Path);
            }
        }

    }
}
