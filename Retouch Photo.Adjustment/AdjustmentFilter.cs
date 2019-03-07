using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retouch_Photo.Adjustments
{
    /// <summary>
    /// 传递AdjustmentFilter的委托。
    /// </summary>
    public delegate void AdjustmentFilterHandler(AdjustmentFilter adjustmentFilter);
  
    /// 从Json形式转为可用的形式
    /// [AdjustmentFilter] --> [List<AdjustmentItem>]
    public class AdjustmentFilter
    {
        public string Name;

        public IEnumerable<AdjustmentItem> AdjustmentItems;


        //@static
        /// <summary> Get List Filter. </summary> 
        public static IEnumerable<AdjustmentFilter> GetFilters(string json)
        {
            // Json --> List<Object>
            IEnumerable<object> objects = JsonConvert.DeserializeObject<IEnumerable<object>>(json);

            // List<Object> --> List<Filter>
            IEnumerable<AdjustmentFilter> filters =
                from item
                in objects
                select AdjustmentFilter.GetFilter(item.ToString());

            return filters;
        }

        //@static
        /// <summary> Get Filter. </summary> 
        public static AdjustmentFilter GetFilter(string json)
        {
            // Json --> Filter2
            AdjustmentFilter2 flter2 = JsonConvert.DeserializeObject<AdjustmentFilter2>(json);

            // Object --> Json -->  List<Item>
            string jsonsssss = flter2.AdjustmentItems.ToString();
            IEnumerable<AdjustmentItem> items = AdjustmentItem.GetItems(jsonsssss);

            // List<Item> -- > Filter
            AdjustmentFilter filter = new AdjustmentFilter()
            {
                Name = flter2.Name,
                AdjustmentItems = items
            };

            return filter;
        }

    }

    /// 为了保护里面的Items，不得已用Object临时保存
    /// [AdjustmentFilter2] --> [AdjustmentFilter]
    public class AdjustmentFilter2
    {
        public string Name;
        public object AdjustmentItems;
    }
}
