using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {
        
        /// <summary>
        /// Save thumbnail image to local folder.
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
        
    }
}