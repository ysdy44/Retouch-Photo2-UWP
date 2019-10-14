using Newtonsoft.Json;
using Retouch_Photo2.Adjustments.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Factory to provide filters and adjustments.
    /// </summary>
    public class FilterFactory: IFilterFactory
    {
        
        public IEnumerable<Filter> CreateFilters(string json)
        {
            // Json --> List<Object>
            IEnumerable<object> objects = JsonConvert.DeserializeObject<IEnumerable<object>>(json);

            // List<Object> --> List<Filter>
            IEnumerable<Filter> filters =
                from item
                in objects
                select this.CreateFilter(item.ToString());// Object --> Json --> Filter


            return filters;
        }
        
        public Filter CreateFilter(string json)
        {
            // Json --> Filter2
            Filter2 flter2 = JsonConvert.DeserializeObject<Filter2>(json);

            // Filter2 --> List<Json> --> List<Adjustment>
            IEnumerable<IAdjustment> items =
            from a
            in flter2.Adjustments
            select this.CreateAdjustment(a.ToString());// Object --> Json --> Adjustment

            // List<Adjustment> -- > Filter
            return new Filter()
            {
                Name = flter2.Name,
                Adjustments = items
            };
        }
        
        public IAdjustment CreateAdjustment(string json)
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
}