using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Retouch_Photo2.Adjustments
{
    //@Delegate
    public delegate void AdjustmentFilterHandler(Filter adjustmentFilter);
    
    /// <summary>
    /// <see cref = "IAdjustment" />'s filter. 
    /// </summary>
    public class Filter
    {        
        [JsonProperty]
        public string Name;

        [JsonProperty]
        public IEnumerable<IAdjustment> Adjustments;


        //@Static
        /// <summary> [Json] --> List [Filter] </summary>
        public static IEnumerable<Filter> GetFiltersFromJson(string json)
        {
            // Json --> List<Object>
            IEnumerable<object> objects = JsonConvert.DeserializeObject<IEnumerable<object>>(json);

            // List<Object> --> List<Filter>
            IEnumerable<Filter> filters =
                from item
                in objects
                select Filter.GetFilterFromJson(item.ToString());// Object --> Json --> Filter


            return filters;
        }
        
        /// <summary> [Json] --> [Filter] </summary>
        private static Filter GetFilterFromJson(string json)
        {
            // Json --> Filter2
            Filter2 flter2 = JsonConvert.DeserializeObject<Filter2>(json);

            // Filter2 --> List<Json> --> List<Adjustment>
            IEnumerable< IAdjustment > items =
            from a
            in flter2.Adjustments
            select Filter.GetAdjustmentFromJson(a.ToString());// Object --> Json --> Adjustment

            // List<Adjustment> -- > Filter
            Filter filter = new Filter()
            {
                Name = flter2.Name,
                Adjustments = items
            };

            return filter;
        }

        /// <summary> [Json] --> [Adjustment] </summary>
        private static IAdjustment GetAdjustmentFromJson(string json)
        {
            // Json --> Adjustment2
            Adjustment2 adjustment2 = JsonConvert.DeserializeObject<Adjustment2>(json);

            // Adjustment2 --> Name
            string typeName = adjustment2.TypeName;

            // Name --> Item
            if (typeName == AdjustmentType.Gray.ToString()) return JsonConvert.DeserializeObject<GrayAdjustment>(json);
            if (typeName == AdjustmentType.Invert.ToString()) return JsonConvert.DeserializeObject<InvertAdjustment>(json);
            if (typeName == AdjustmentType.Exposure.ToString()) return JsonConvert.DeserializeObject<ExposureAdjustment>(json);
            if (typeName == AdjustmentType.Brightness.ToString()) return JsonConvert.DeserializeObject<BrightnessAdjustment>(json);
            if (typeName == AdjustmentType.Saturation.ToString()) return JsonConvert.DeserializeObject<SaturationAdjustment>(json);
            if (typeName == AdjustmentType.HueRotation.ToString()) return JsonConvert.DeserializeObject<HueRotationAdjustment>(json);
            if (typeName == AdjustmentType.Contrast.ToString()) return JsonConvert.DeserializeObject<ContrastAdjustment>(json);
            if (typeName == AdjustmentType.Temperature.ToString()) return JsonConvert.DeserializeObject<TemperatureAdjustment>(json);
            if (typeName == AdjustmentType.HighlightsAndShadows.ToString()) return JsonConvert.DeserializeObject<HighlightsAndShadowsAdjustment>(json);
            if (typeName == AdjustmentType.GammaTransfer.ToString()) return JsonConvert.DeserializeObject<GammaTransferAdjustment>(json);
            if (typeName == AdjustmentType.Vignette.ToString()) return JsonConvert.DeserializeObject<VignetteAdjustment>(json);

            return new GrayAdjustment();
        }


        /// <summary>
        /// Read the filter collection from the Filter.json.
        /// </summary>
        /// <returns> The default filters. </returns>
        public static async Task<IEnumerable<Filter>> GetFilterSource()
        {
            string json = await Filter.ReadFromLocalFolder("Filter.json");

            if (json == null)
            {
                json = await Filter.ReadFromApplicationPackage("ms-appx:///Json/Filter.json");
                Filter.WriteToLocalFolder(json, "ms-appx:///Json/Filter.json");
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

    /// <summary> <see cref = "Filter" />'s substitute. </summary>
    public class Filter2
    {
        [JsonProperty]
        public string Name;

        [JsonProperty]
        public IEnumerable<object> Adjustments;
    }
}
