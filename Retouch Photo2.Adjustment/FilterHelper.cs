using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide, cache, read, write filters.
    /// </summary>
    public static class FilterHelper
    {

        /// <summary>
        /// Read the filter collection from the Filter.json.
        /// </summary>
        /// <returns> The default filters. </returns>
        public static async Task<IEnumerable<Filter>> GetFilterSource()
        {
            string json = await FilterHelper.ReadFromLocalFolder("Filter.json");

            if (json == null)
            {
                json = await FilterHelper.ReadFromApplicationPackage("ms-appx:///Json/Filter.json");
                FilterHelper.WriteToLocalFolder(json, "ms-appx:///Json/Filter.json");
            }
            IEnumerable<Filter> source = Filter.GetFiltersFromJson(json);

            return source;
        }
        /// <summary> 
        /// Read json file from Application Package. 
        /// </summary> 
        /// <param name="fileName"></param>
        /// <returns> The default json. </returns>
        private static async Task<string> ReadFromApplicationPackage(string fileName)
        {
            Uri uri = new Uri(fileName);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await FileIO.ReadTextAsync(file);
        }

        /// <summary>
        /// Read json file from Local Folder. 
        /// </summary> 
        /// <param name="fileName"> The source file name. </param>
        /// <returns> The default json. </returns>
        private static async Task<string> ReadFromLocalFolder(string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Write json file to Local Folder. 
        /// </summary> 
        /// <param name="json"> The source json. </param>
        /// <param name="fileName"> The source file name. </param>
        public static async void WriteToLocalFolder(string json, string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, json);
            }
            catch (Exception)
            {
            }
        }
    }
}
