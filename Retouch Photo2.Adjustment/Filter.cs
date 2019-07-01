using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Adjustments
{
    //@Delegate
    public delegate void AdjustmentFilterHandler(Filter adjustmentFilter);
    
    /// <summary>
    /// <see cref = "Adjustment" />'s filter. 
    /// </summary>
    public class Filter
    {        
        public string Name;

        public IEnumerable<AdjustmentItem> AdjustmentItems;

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

        //@Static
        /// <summary> [Json] --> [Filter] </summary>
        public static Filter GetFilterFromJson(string json)
        {
            // Json --> Filter2
            Filter2 flter2 = JsonConvert.DeserializeObject<Filter2>(json);

            // Filter2 --> Json
            string jsonsssss = flter2.AdjustmentItems.ToString();

            // Json --> List<Item>
            IEnumerable<AdjustmentItem> items = AdjustmentItem.GetItemsFromJson(jsonsssss);

            // List<Item> -- > Filter
            Filter filter = new Filter()
            {
                Name = flter2.Name,
                AdjustmentItems = items
            };

            return filter;
        }
    }

    /// <summary> <see cref = "Filter" />'s substitute. </summary>
    public class Filter2
    {
        public string Name;
        public object AdjustmentItems;
    }
}
