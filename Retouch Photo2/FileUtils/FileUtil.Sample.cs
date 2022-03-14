using Microsoft.Graphics.Canvas;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Save samples files (Open from Application, Save from local folder).
        /// </summary>
        public static async Task SaveSampleFile()
        {
            // Read the file from the package.
            StorageFile file0 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/App.photo2pk.zip"));
            StorageFile file1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/Banner.photo2pk.zip"));
            StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///FileUtils/MFY.photo2pk.zip"));

            // Unzip to local folder
            await FileUtil.ExtractToDirectory(file0);
            await FileUtil.ExtractToDirectory(file1);
            await FileUtil.ExtractToDirectory(file2);
        }
        /// <summary>
        /// Load project and extract zip to local folder.
        /// </summary>
        /// <param name="file"> The file. </param>
        /// <returns> The extract project. </returns>
        private static async Task ExtractToDirectory(StorageFile file)
        {
            // Read the file stream
            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                // Unzip to local folder
                ZipArchive archive = new ZipArchive(stream);
                archive.ExtractToDirectory(ApplicationData.Current.LocalFolder.Path);
            }
        }


        /// <summary>
        /// Save thumbnail file to zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder.</param>
        /// <param name="renderTarget"> The render target.</param>
        /// <returns> Return image source. </returns>
        public static async Task<WriteableBitmap> SaveThumbnailFile(StorageFolder zipFolder, CanvasRenderTarget renderTarget)
        {
            StorageFile thumbnailFile = await zipFolder.CreateFileAsync("Thumbnail.png");

            using (IRandomAccessStream stream = await thumbnailFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await renderTarget.SaveAsync(stream, CanvasBitmapFileFormat.Png);

                return await FileUtil.GetImageSource(stream);
            }
        }
        /// <summary>
        /// Get image source by steam.
        /// </summary>
        /// <param name="stream"> The steam. </param>
        /// <returns> Return image source. </returns>
        private static async Task<WriteableBitmap> GetImageSource(IRandomAccessStream stream)
        {
            // Display
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

            using (SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied))
            {
                WriteableBitmap source = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
                bitmap.CopyToBuffer(source.PixelBuffer);
                return source;
            }
        }


        /// <summary>
        /// Saves the entire bitmap to the specified stream
        /// with the specified file format and quality level.
        /// </summary>
        /// <param name="renderTarget"> The render target.</param>
        /// <param name="fileChoices"> The file choices. </param>
        /// <param name="suggestedFileName"> The suggested name of file. </param>
        /// <param name="fileFormat"> The file format. </param>
        /// <param name="quality"> The file quality. </param>
        /// <returns> Saved successful? </returns>
        public static async Task<bool> SaveCanvasBitmapFile(CanvasRenderTarget renderTarget, string fileChoices = ".Jpeg", string suggestedFileName = "Untitled", CanvasBitmapFileFormat fileFormat = CanvasBitmapFileFormat.Jpeg, float quality = 1.0f)
        {
            // FileSavePicker
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = suggestedFileName,
                FileTypeChoices =
                {
                    {"DB", new[] { fileChoices } }
                }
            };


            // PickSaveFileAsync
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file is null) return false;

            try
            {
                using (IRandomAccessStream accessStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await renderTarget.SaveAsync(accessStream, fileFormat, quality);
                }

                renderTarget.Dispose();
                return true;
            }
            catch (Exception)
            {
                renderTarget.Dispose();
                return false;
            }
        }

    }
}