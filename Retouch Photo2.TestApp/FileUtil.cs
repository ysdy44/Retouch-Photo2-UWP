using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        #region ZipFile: Extract & Create


        /// <summary>
        ///  Extract zip file from local folder to temporary folder.
        /// </summary>
        /// <param name="zipFilePath"> The path of zip file. </param>
        /// <returns> The extract project. </returns>
        public static async Task ExtractZipFile(string zipFilePath)
        {
            //Read the folder
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(zipFilePath);

            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (var item in files)
            {
                //Unzip to temporary folder
                await item.CopyAsync(ApplicationData.Current.TemporaryFolder);
            }
        }

        /// <summary>
        /// Create a zip file from temporary folder to local folder.
        /// </summary>
        /// <param name="name"> The zip file name. </param>
        public static async Task CreateZipFile(string name)
        {
            bool isFileExists = await ApplicationLocalTextFileUtility.IsFileExistsInLocalFolder($"{name}.photo2pk");

            StorageFolder folder = null;

            if (isFileExists)
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync($"{name}.photo2pk");

                //Delete if it exists in local folder.
                IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                foreach (var file in files)
                {
                    await file.DeleteAsync();
                }
            }
            else
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync($"{name}.photo2pk");
            }

            //Move
            {
                //Zip all file in temporary folder to local folder.
                IReadOnlyList<StorageFile> files = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();

                foreach (var item in files)
                {
                    //Unzip to temporary folder
                    await item.MoveAsync(folder);
                }
            }
        }


        #endregion


        /// <summary>
        /// The file picker is displayed so that the user can select a file.
        /// Then copy to the temporary folder, and return the copy
        /// </summary>
        /// <param name="location"> The destination LocationId. </param>
        /// <returns> The product file. </returns>
        public async static Task<StorageFile> PickAndCopySingleImageFileAsync(PickerLocationId location)
        {
            //Picker
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = location,
                FileTypeFilter =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".bmp"
                }
            };

            //File
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return null;

            StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
            return copyFile;
        }

        /// <summary>
        /// Create a <see cref="Photo"/> form a copy which in the temporary folder.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="copyFile"> The copy file. </param>
        /// <returns> The product photo. </returns>
        public async static Task<Photo> CreatePhotoFromCopyFileAsync(ICanvasResourceCreator resourceCreator, StorageFile copyFile)
        {
            using (IRandomAccessStream stream = await copyFile.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream);

                return new Photo
                {
                    Source = bitmap,
                    ImageFilePath = copyFile.Path,

                    Name = copyFile.DisplayName,
                    FileType = copyFile.FileType,
                    FolderRelativeId = copyFile.FolderRelativeId,
                };
            }
        }

        /// <summary>
        /// Saves the entire bitmap to the specified stream with the specified file format and quality level.
        /// </summary>
        /// <param name="renderTarget"> The render target.</param>
        /// <param name="fileChoices"> The file choices. </param>
        /// <param name="suggestedFileName"> The suggested name of file. </param>
        /// <param name="fileFormat"> The file format. </param>
        /// <param name="quality"> The file quality. </param>
        /// <returns> Saved successful? </returns>
        public static async Task<bool> ExportStorageFile(CanvasRenderTarget renderTarget, string fileChoices = ".Jpeg", string suggestedFileName = "Untitled", CanvasBitmapFileFormat fileFormat = CanvasBitmapFileFormat.Jpeg, float quality = 1.0f)
        {
            //FileSavePicker
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = suggestedFileName,
            };
            savePicker.FileTypeChoices.Add("DB", new[] { fileChoices });


            //PickSaveFileAsync
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file == null) return false;

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