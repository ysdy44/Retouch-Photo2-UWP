using Retouch_Photo2.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Retouch_Photo2
{
    public static partial class XML
    {

        public static Project LoadProjectFile(string name)
        {
            //Create an XDocument object.
            string path = $"{ApplicationData.Current.TemporaryFolder.Path}\\Project.xml";
            XDocument document = XDocument.Load(path);

            Project project = Retouch_Photo2.ViewModels.XML.LoadProject(name, document);
            return project;
        }

        public static async Task SaveProjectFile(StorageFolder zipFolder, Project project)
        {
            XDocument document = Retouch_Photo2.ViewModels.XML.SaveProject(project);

            //Save the project file.      
            StorageFile file = await zipFolder.CreateFileAsync("Project.xml", CreationCollisionOption.ReplaceExisting);
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