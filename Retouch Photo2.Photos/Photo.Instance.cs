using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Photos
{
    /// <summary>
    /// Represents a photo that contains dpi, width, height information.
    /// </summary>
    public partial class Photo : UserControl
    {

        //@Static
        /// <summary> Dictionary <see cref="Photo"/>s instances. </summary>
        public static readonly Dictionary<string, Photo> Instances = new Dictionary<string, Photo>();

        /// <summary> Collection <see cref="Photo"/>s instances. </summary>
        public readonly static ObservableCollection<Photo> InstancesCollection = new ObservableCollection<Photo>();

        /// <summary>
        /// Update collection <see cref="Photo"/>s instances.
        /// </summary>
        public static void UpdateInstancesCollection()
        {
            Photo.InstancesCollection.Clear();
            foreach (Photo photo in Instances.Values)
            {
                Photo.InstancesCollection.Add(photo);
            }
        }


        /// <summary>
        /// Check duplicate <see cref="Photo"/>.
        /// If it exists, replace it, or insert it into the <see cref="Photo"/>s.
        /// </summary>
        /// <param name="photo"> The source photo. </param>
        public static void DuplicateChecking(Photo photo)
        {
            string id = photo.FolderRelativeId;

            if (Photo.Instances.ContainsKey(id))
            {
                photo = Photo.Instances[id];
                return;
            }

            Photo.Instances.Add(id, photo); // Photos
            Photo.UpdateInstancesCollection();
        }

        /// <summary>
        /// Find the first <see cref="Photo"/> by <see cref="Photocopier"/>.
        /// </summary>
        /// <param name="photocopier"> The source photocopier</param>
        /// <returns> The product photo. </returns>
        public static Photo FindFirstPhoto(Photocopier photocopier)
        {
            string id = photocopier.FolderRelativeId;
            return Photo.Instances[id];
        }


        /// <summary>
        /// Move file to zip folder.
        /// </summary>
        /// <param name="zipFolder"> the zip folder. </param>
        public async Task MoveFile(StorageFolder zipFolder)
        {
            // Move photo file.
            StorageFile item = await StorageFile.GetFileFromPathAsync(this.ImageFilePath);
            await item.CopyAsync(zipFolder);
        }


        /// <summary>
        /// Construct the <see cref="Photo.Source"/> by self.
        /// </summary>
        /// <param name="resourceCreator"> The resource creator. </param>
        public async Task ConstructPhotoSource(ICanvasResourceCreator resourceCreator)
        {
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\{this.Name}{this.FileType}";
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            if (file is null) return;

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