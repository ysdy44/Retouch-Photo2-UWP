using Newtonsoft.Json;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo.Adjustments
{
    /// 从Json形式转为可用的形式
    /// [AdjustmentItem] --> [Adjustment]
    public class AdjustmentItem
    {
        public AdjustmentType Type;

        public virtual Adjustment GetAdjustment() => new GrayAdjustment();


        //@static
        /// <summary> Get List Item. </summary>
        public static IEnumerable<AdjustmentItem> GetItems(string json)
        {
            // Json -->  List<object>
            IEnumerable<object> objects = JsonConvert.DeserializeObject<IEnumerable<object>>(json);

            // List<object> -- > List<Item>
            IEnumerable<AdjustmentItem> items =
                from item
                in objects
                select AdjustmentItem.GetItem(item.ToString());

            return items;
        }
     
        //@static
        /// <summary> Get Item. </summary>
        public static AdjustmentItem GetItem(string json)
        {
            AdjustmentItem adjustmentItem = JsonConvert.DeserializeObject<AdjustmentItem>(json);

            switch (adjustmentItem.Type)
            {
                case AdjustmentType.Gray: return JsonConvert.DeserializeObject<GrayAdjustmentItem>(json);
                case AdjustmentType.Invert: return JsonConvert.DeserializeObject<InvertAdjustmentItem>(json);
                case AdjustmentType.Exposure: return JsonConvert.DeserializeObject<ExposureAdjustmentItem>(json);
                case AdjustmentType.Brightness: return JsonConvert.DeserializeObject<BrightnessAdjustmentItem>(json);
                case AdjustmentType.Saturation: return JsonConvert.DeserializeObject<SaturationAdjustmentItem>(json);
                case AdjustmentType.HueRotation: return JsonConvert.DeserializeObject<HueRotationAdjustmentItem>(json);
                case AdjustmentType.Contrast: return JsonConvert.DeserializeObject<ContrastAdjustmentItem>(json);
                case AdjustmentType.Temperature: return JsonConvert.DeserializeObject<TemperatureAdjustmentItem>(json);
                case AdjustmentType.HighlightsAndShadows: return JsonConvert.DeserializeObject<HighlightsAndShadowsAdjustmentItem>(json);
                case AdjustmentType.GammaTransfer: return JsonConvert.DeserializeObject<GammaTransferAdjustmentItem>(json);
                case AdjustmentType.Vignette: return JsonConvert.DeserializeObject<VignetteAdjustmentItem>(json);
                default: return adjustmentItem;
            }
        }
    }
}
