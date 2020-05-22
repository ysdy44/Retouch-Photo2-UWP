using Retouch_Photo2.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class XML
    {


        /// <summary>
        /// Load <see cref="Layer"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product layers. </returns>
        public static IEnumerable<ILayer> LoadLayersFile()
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}/Layers.xml";

            try
            {
                XDocument document = XDocument.Load(path);

                IEnumerable<ILayer> layers = Retouch_Photo2.Layers.XML.LoadLayers(document);
                return layers;
            }
            catch (Exception)
            {
                IEnumerable<ILayer> layers = new List<ILayer>();
                return layers;
            }
        }


        public static async Task SaveLayerFile(StorageFolder zipFolder, IEnumerable<ILayer> layers)
        {
            XDocument document = Retouch_Photo2.Layers.XML.SaveLayers(layers);

            //Save the project xml file.      
            StorageFile file = await zipFolder.CreateFileAsync("Layers.xml", CreationCollisionOption.ReplaceExisting);
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
