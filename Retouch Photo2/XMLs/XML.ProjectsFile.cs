// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
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
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Construct <see cref="Project"/>s File (Open from Defult, Save from LocalFolder)
        /// </summary>
        /// <returns> The product <see cref="Project"/>s. </returns>
        public static async Task<IEnumerable<Project>> ConstructProjectsFile()
        {
            StorageFile file = null;
            bool isLocalProjectsExists = await FileUtil.IsFileExistsInLocalFolder("Projects.xml");

            if (isLocalProjectsExists)
            {
                // Read the file from the local folder.
                file = await ApplicationData.Current.LocalFolder.GetFileAsync("Projects.xml");
            }
            else
            {
                // Read the file from the package.
                file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///XMLs/Projects.xml"));

                // Copy to the local folder.
                await file.CopyAsync(ApplicationData.Current.LocalFolder);
            }

            if (file == null) return null;

            using (Stream stream = await file.OpenStreamForReadAsync())
            {
                try
                {
                    XDocument document = XDocument.Load(stream);

                    IEnumerable<Project> source = Retouch_Photo2.ViewModels.XML.LoadProjects(document);
                    return source;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Save <see cref="Project"/>s to local folder.
        /// </summary>
        /// <param name="projects"> The projects. </param>
        public static async Task SaveProjectsFile(IEnumerable<Project> projects)
        {
            XDocument document = Retouch_Photo2.ViewModels.XML.SaveProjects(projects);

            // Save the Project xml file.      
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Projects.xml", CreationCollisionOption.ReplaceExisting);
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