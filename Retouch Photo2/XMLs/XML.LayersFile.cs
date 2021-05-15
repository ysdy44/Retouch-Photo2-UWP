// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Microsoft.Graphics.Canvas;
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
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Load <see cref="ILayer"/>s file from temporary folder.
        /// </summary>
        /// <returns> The product <see cref="ILayer"/>s. </returns>
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
                return null;
            }
        }
        
        /// <summary>
        /// Save <see cref="ILayer"/>s to zip folder.
        /// </summary>
        /// <param name="zipFolder"> The zip folder. </param>
        /// <param name="layers"> The layers. </param>
        public static async Task SaveLayersFile(StorageFolder zipFolder, IEnumerable<ILayer> layers)
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
