using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// <see cref = "IAdjustment" />'s filter. 
    /// </summary>
    public class Filter
    {        
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public IEnumerable<IAdjustment> Adjustments { get; set; }


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
            IEnumerable<IAdjustment> items =
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
    }

    /// <summary> <see cref = "Filter" />'s substitute. </summary>
    public class Filter2
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public IEnumerable<object> Adjustments { get; set; }
    }
}
