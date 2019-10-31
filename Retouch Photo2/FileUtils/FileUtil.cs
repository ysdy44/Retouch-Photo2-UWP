using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Find all photo file.
        /// </summary>
        /// <returns> The all project file. </returns>
        public static async Task<IEnumerable<StorageFile>> FindPhoto2pkFile()
        {
            //get all file.
            IReadOnlyList<StorageFile> files = await ApplicationData.Current.LocalFolder.GetFilesAsync();

            //Sort by Time
            IOrderedEnumerable<StorageFile> orderedFiles = files.OrderByDescending(file => file.DateCreated);

            //Ordered
            IEnumerable<StorageFile> orderedPhotos = from flie in orderedFiles where flie.FileType == ".photo2pk" select flie;
            return orderedPhotos;
        }

        /// <summary>
        /// Save thumbnail file to local folder.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="renderAction"> The render-action. </param>
        /// <param name="name"> The file name.</param>
        /// <param name="width"> The thumbnail width.</param>
        /// <param name="height"> The thumbnail height.</param>
        public static async void SaveThumbnailAsync(ICanvasResourceCreator resourceCreator, Func<Matrix3x2, ICanvasImage> renderAction, string name = "untitled", int width = 256, int height = 256)
        {
            float scale = 1;
            int thumbnailWidth = 256;
            int thumbnailHeight = 256;
            if (width > height)
            {
                scale = 256.0f / width;
                thumbnailHeight = (int)(scale * height);
            }
            else
            {
                scale = 256.0f / height;
                thumbnailWidth = (int)(scale * width);
            }

            //Thumbnail
            CanvasRenderTarget thumbnail = new CanvasRenderTarget(resourceCreator, thumbnailWidth, thumbnailHeight, 96);
            {
                Matrix3x2 matrix = Matrix3x2.CreateScale(scale);
                ICanvasImage previousImage = renderAction(matrix);

                using (CanvasDrawingSession drawingSession = thumbnail.CreateDrawingSession())
                {
                    drawingSession.DrawImage(previousImage);
                }
            }

            //File
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync($"{name}.png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await thumbnail.SaveAsync(fileStream, CanvasBitmapFileFormat.Png);
            }
        }

        /// <summary>
        /// Create a Image form a LocationId.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="location"> The destination LocationId. </param>
        /// <returns> The product ImageRe. </returns>
        public async static Task<ImageRe> CreateFromLocationIdAsync(ICanvasResourceCreator resourceCreator, PickerLocationId location)
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

            //ImageRe
            using (IRandomAccessStream stream = await file.OpenReadAsync())
            {
                CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(resourceCreator, stream);

                return new ImageRe
                {
                    Source = bitmap,
                    Name = file.DisplayName,
                    FileType = file.FileType,
                    FolderRelativeId = file.FolderRelativeId,
                };
            }
        }


    }
}