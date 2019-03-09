using Newtonsoft.Json;
using Retouch_Photo.Adjustments.Items;
using Retouch_Photo.Adjustments.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo.Adjustments
{
    /// 从Json形式转为可用的形式
    /// [AdjustmentItem] --> [Adjustment]
    public abstract class AdjustmentItem
    {
        public string Name;

        public abstract Adjustment GetAdjustment();


        //@static
        /// <summary> Get List Item. </summary>
        public static IEnumerable<AdjustmentItem> GetItemsFromJson(string json)
        {
            // Json -->  List<object>
            IEnumerable<object> objects = JsonConvert.DeserializeObject<IEnumerable<object>>(json);

            // List<object> -- > List<Item>
            IEnumerable<AdjustmentItem> items =
                from item
                in objects
                select AdjustmentItem.GetItemFromJson(item.ToString());

            return items;
        }
     
        //@static
        /// <summary> Get Item. </summary>
        public static AdjustmentItem GetItemFromJson(string json)
        {
            // Josn --> Item2 --> Type
            AdjustmentItem2 adjustmentItem2 = JsonConvert.DeserializeObject<AdjustmentItem2>(json);
            string name = adjustmentItem2.Name;

            // Name --> Item
            if (name == GrayAdjustment.Name) return JsonConvert.DeserializeObject<GrayAdjustmentItem>(json);
            if (name == InvertAdjustment.Name) return JsonConvert.DeserializeObject<InvertAdjustmentItem>(json);
            if (name == ExposureAdjustment.Name) return JsonConvert.DeserializeObject<ExposureAdjustmentItem>(json);
            if (name == BrightnessAdjustment.Name) return JsonConvert.DeserializeObject<BrightnessAdjustmentItem>(json);
            if (name == SaturationAdjustment.Name) return JsonConvert.DeserializeObject<SaturationAdjustmentItem>(json);
            if (name == HueRotationAdjustment.Name) return JsonConvert.DeserializeObject<HueRotationAdjustmentItem>(json);
            if (name == ContrastAdjustment.Name) return JsonConvert.DeserializeObject<ContrastAdjustmentItem>(json);
            if (name == TemperatureAdjustment.Name) return JsonConvert.DeserializeObject<TemperatureAdjustmentItem>(json);
            if (name == HighlightsAndShadowsAdjustment.Name) return JsonConvert.DeserializeObject<HighlightsAndShadowsAdjustmentItem>(json);
            if (name == GammaTransferAdjustment.Name) return JsonConvert.DeserializeObject<GammaTransferAdjustmentItem>(json);
            if (name == VignetteAdjustment.Name) return JsonConvert.DeserializeObject<VignetteAdjustmentItem>(json);

            return new GrayAdjustmentItem();
        }
    }

    /// 为了得到里面的Type，不得已用而实例化它
    /// [Item2] --> [Item]
    public class AdjustmentItem2
    {
        public string Name;
    }
}
