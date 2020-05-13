using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class FileUtil
    {

        /// <summary>
        /// Delete all files in temp folder.
        /// </summary>
        public static async Task DeleteAllInTemporaryFolder()
        {
            IReadOnlyList<StorageFile> items = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            foreach (StorageFile item in items)
            {
                await item.DeleteAsync();
            }
        }


        #region Photo: Load & Save


        /// <summary>
        /// Load <see cref="Photo"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product photos. </returns>
        public static IEnumerable<Photo> LoadPhotoFile()
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/Photos.xml";
            XDocument document = XDocument.Load(path);

            IEnumerable<Photo> photos = Retouch_Photo2.Elements.XML.LoadPhotos(document);
            return photos;
        }

        /// <summary>
        /// Save <see cref="Photo"/>s file to temporary folder.
        /// </summary>
        /// <param name="photos"> The source photos. </param>
        public static async Task SavePhotoFile(IEnumerable<Photo> photos)
        {
            XDocument document = Retouch_Photo2.Elements.XML.SavePhotos(photos);

            //Save the project xml file.      
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Photos.xml", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (Stream stream = fileStream.AsStream())
                {
                    document.Save(stream);
                }
            }
        }


        /// <summary>
        /// Construct a <see cref="Photo"/>
        /// (Source and ImageFilePath).
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="photo"> The source photo. </param>
        public static async Task ConstructPhotoAndPushInstances(ICanvasResourceCreator resourceCreator, Photo photo)
        {
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\{photo.Name}{photo.FileType}";
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                photo.Source = await CanvasBitmap.LoadAsync(resourceCreator, fileStream);
                photo.ImageFilePath = path;
            }
        }

        /// <summary>
        /// Get useful <see cref="Photo"/>s and Delete useless
        /// (A <see cref="Photo"/> would be useless if it could't be found in saved <see cref="Photocopier"/>s).
        /// </summary>
        /// <param name="allPhotos"> The all photos. </param>
        /// <param name="savedPhotocopiers"> The saved photocopiers. </param>
        /// <returns> The useful photos. </returns>
        public static IEnumerable<Photo> GetPhotosAndDeleteUseless(IEnumerable<Photo> allPhotos, IEnumerable<Photocopier> savedPhotocopiers)
        {
            foreach (Photo photo in allPhotos)
            {
                bool isAnyEquals = savedPhotocopiers.Any(p => photo.Equals(p));

                if (isAnyEquals)
                {
                    //Get saved Photo
                    yield return photo;
                }
                else
                {
                    //Delete useless.
                    FileUtil.DeletePhotoImageFilePath(photo);
                }
            }
        }


        /// <summary>
        /// Delete <see cref="Photo"/> ImageFilePath
        /// </summary>
        /// <param name="photo"> The source photo. </param>
        public static async void DeletePhotoImageFilePath(Photo photo)
        {
            string path = photo.ImageFilePath;
            StorageFile item = await StorageFile.GetFileFromPathAsync(path);
            await item.DeleteAsync();
        }



        #endregion


        #region Project: Load & Save


        /// <summary>
        /// Load <see cref="Project"/> from temporary folder.
        /// </summary>
        /// <param name="name"> The project name. </param>
        /// <returns> The loaded project. </returns>
        public static Project LoadProject(string name)
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\Project.xml";
            XDocument document = XDocument.Load(path);

            Project project = Retouch_Photo2.ViewModels.XML.LoadProject(name, document);
            return project;
        }

        /// <summary>
        /// Save <see cref="Project"/> to temporary folder.
        /// </summary>
        /// <param name="project"> The project. </param>
        public static async Task SaveProject(Project project)
        {
            XDocument document = Retouch_Photo2.ViewModels.XML.SaveProject(project);

            //Save the project xml file.      
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Project.xml", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (Stream stream = fileStream.AsStream())
                {
                    document.Save(stream);
                }
            }
        }


        #endregion


    }
}