using Microsoft.Graphics.Canvas;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image of <see cref="Photo">.
    /// </summary>
    public partial class Photo
    {

        //@Static
        /// <summary> Collection <see cref="Photo"/>s instances. </summary>
        public static ObservableCollection<Photo> Instances = new ObservableCollection<Photo>();
        
        /// <summary>
        /// Check duplicate <see cref="Photo"/>.
        /// If it exists, replace it, or insert it into the <see cref="Photo"/>s.
        /// </summary>
        /// <param name="photo"> The source photo. </param>
        public static void DuplicateChecking(Photo photo)
        {
            foreach (Photo p in Photo.Instances)
            {
                if (p.FolderRelativeId == photo.FolderRelativeId)
                {
                    photo = p;
                    return;
                }
            }

            Photo.Instances.Add(photo);//Photos
        }
        
        /// <summary>
        /// Find the first <see cref="Photo"/> by <see cref="Photocopier"/>.
        /// </summary>
        /// <param name="photocopier"> The source photocopier</param>
        /// <returns> The product photo. </returns>
        public static Photo FindFirstPhoto(Photocopier photocopier)
        {
            string id = photocopier.FolderRelativeId;
            return Photo.Instances.FirstOrDefault(i => i.FolderRelativeId == id);
        }


        /// <summary>
        /// Move file to zip folder.
        /// </summary>
        /// <param name="zipFolder"> the zip folder. </param>
        public async Task MoveFile(StorageFolder zipFolder)
        {
            //Move photo file.
            StorageFile item = await StorageFile.GetFileFromPathAsync(this.ImageFilePath);
            await item.MoveAsync(zipFolder);
        }
        

        /// <summary>
        /// Construct the <see cref="Photo.Source"/> by self.
        /// </summary>
        /// <param name=""></param>
        /// <param name="resourceCreator"> The resource creator. </param>
        /// <param name="photo"></param>
        /// <returns></returns>
        public async Task ConstructPhotoSource(ICanvasResourceCreator resourceCreator)
        {
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\{this.Name}{this.FileType}";
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            if (file == null) return;

            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                this.Source = await CanvasBitmap.LoadAsync(resourceCreator, fileStream);
                this.ImageFilePath = path;
            }
        }


        /// <summary>
        /// Create a <see cref="Photo"/> form a copied file.
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


    }
}
