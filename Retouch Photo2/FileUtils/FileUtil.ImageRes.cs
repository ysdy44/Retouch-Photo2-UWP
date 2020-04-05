using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {
        
        /// <summary>
        /// Load <see cref="ImageRe"/>s from temporary folder.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns></returns>
        public static async Task LoadImageRes(ICanvasResourceCreator resourceCreator)
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/imageRes.xml";
            XDocument document = XDocument.Load(path);

            IEnumerable<ImageRe> imageRes = Retouch_Photo2.Elements.XML.LoadImageRes(document);

            //Load all images.
            ImageRe.Instances.Clear();
            foreach (ImageRe imageRe in imageRes)
            {
                StorageFile file = await ApplicationData.Current.TemporaryFolder.GetFileAsync($"{imageRe.Name}{imageRe.FileType}");
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    imageRe.Source = await CanvasBitmap.LoadAsync(resourceCreator, fileStream);
                    ImageRe.Instances.Push(imageRe);
                }
            }
        }

        /// <summary>
        /// Save <see cref="ImageRe"/>s to temporary folder.
        /// </summary>
        public static async Task SaveImageRes()
        {
            IEnumerable<ImageRe> imageRes = ImageRe.Instances;
            XDocument document = Retouch_Photo2.Elements.XML.SaveImageRes(imageRes);
            
            //Save the project xml file.      
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("imageRes.xml", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (Stream stream = fileStream.AsStream())
                {
                    document.Save(stream);
                }
            }

            //Save all canvas bitmap.            
            foreach (ImageRe imageRe in imageRes)
            {
                try
                {
                    string Path = $"{ApplicationData.Current.TemporaryFolder.Path}\\{imageRe.Name}{imageRe.FileType}";
                    CanvasBitmap canvasBitmap = imageRe.Source;
                    await canvasBitmap.SaveAsync(Path);
                }
                catch (Exception) { }
            }
        }
        

        /// <summary>
        /// Create a ImageRe form a LocationId.
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