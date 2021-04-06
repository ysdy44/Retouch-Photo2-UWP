using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Copy file to the temporary folder, and return the copy.
        /// </summary>
        /// <param name="file"> The destination file. </param>
        /// <returns> The copied  file. </returns>
        public async static Task<StorageFile> CopySingleImageFileAsync(StorageFile file)
        {
            if (file == null) return null;

            StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
            return copyFile;
        }

        /// <summary>
        /// The file picker is displayed so that the user can select a file.
        /// </summary>
        /// <param name="location"> The destination locationId. </param>
        /// <returns> The product file. </returns>
        public async static Task<StorageFile> PickSingleImageFileAsync(PickerLocationId location)
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
            return file;
        }

        /// <summary>
        /// The files picker is displayed so that the user can select a files.
        /// </summary>
        /// <param name="location"> The destination locationId. </param>
        /// <returns> The product files. </returns>
        public async static Task<IReadOnlyList<StorageFile>> PickMultipleImageFilesAsync(PickerLocationId location)
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
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            return files;
        }


        /// <summary>
        /// Filter files that are not pictures.
        /// Then copy to the temporary folder, and return the copy.
        /// </summary>
        /// <param name="item"> The destination item. </param>
        /// <returns> The product file. </returns>
        public async static Task<StorageFile> CopySingleImageFileAsync(IStorageItem item)
        {
            if (item is StorageFile file)
            {
                if (file == null) return null;

                string fileType = file.FileType.ToUpper();
                switch (fileType)
                {
                    case ".JPG":
                    case ".JPEG":
                    case ".PNG":
                    case ".GIF":
                    case ".BMP":
                        StorageFile copyFile = await file.CopyAsync(ApplicationData.Current.TemporaryFolder, file.Name, NameCollisionOption.ReplaceExisting);
                        return copyFile;
                }
            }
            return null;
        }

    }
}