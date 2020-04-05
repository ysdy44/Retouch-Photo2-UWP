using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Layers;
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
        /// Load <see cref="Project"/> from temporary folder.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="name"> The project name. </param>
        /// <returns> The loaded project. </returns>
        public static Project LoadProject(ICanvasResourceCreator resourceCreator, string name)
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
        /// <param name="layers"></param>
        /// <param name="name"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static async Task SaveProject(IEnumerable<ILayer> layers, string name = "untitled", int width = 256, int height = 256)
        {
            Project project = new Project
            {
                Name = name,
                Width = width,
                Height = height,
                Layers = layers,
            };
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
                
    }
}