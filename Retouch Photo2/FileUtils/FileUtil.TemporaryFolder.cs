using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Elements;
using Retouch_Photo2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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


        #region ImageRes: Load & Save


        /// <summary>
        /// Load <see cref="ImageRe"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product ImageRes. </returns>
        public static IEnumerable<ImageRe> LoadImageResFile()
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/imageRes.xml";
            XDocument document = XDocument.Load(path);

            IEnumerable<ImageRe> imageRes = Retouch_Photo2.Elements.XML.LoadImageRes(document);
            return imageRes;
        }

        /// <summary>
        /// Save <see cref="ImageRe"/>s file to temporary folder.
        /// </summary>
        /// <param name="imageRes"> The source imageRes. </param>
        public static async Task SaveImageResFile(IEnumerable<ImageRe> imageRes)
        {
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
        }


        /// <summary>
        /// Construct a ImageRe(Source and ImageFilePath).
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="imageRe"> The source imageRe. </param>
        public static async Task ConstructImageReAndPushInstances(ICanvasResourceCreator resourceCreator, ImageRe imageRe)
        {
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\{imageRe.Name}{imageRe.FileType}";
            StorageFile file = await StorageFile.GetFileFromPathAsync(path);
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                imageRe.Source = await CanvasBitmap.LoadAsync(resourceCreator, fileStream);
                imageRe.ImageFilePath = path;
            }
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
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\config.xml";
            XDocument document = XDocument.Load(path);

            Project project = Retouch_Photo2.ViewModels.XML.LoadProject(name, document);
            return project;
        }

        /// <summary>
        /// Save <see cref="Project"/> to temporary folder.
        /// </summary>
        /// <param name="project"> The project. </param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static async Task SaveProject(Project project)
        {
            XDocument document = Retouch_Photo2.ViewModels.XML.SaveProject(project);

            //Save the project xml file.      
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("config.xml", CreationCollisionOption.ReplaceExisting);
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