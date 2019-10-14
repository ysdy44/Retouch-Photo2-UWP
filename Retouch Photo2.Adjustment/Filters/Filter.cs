using Newtonsoft.Json;
using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// <see cref = "IAdjustment" />'s filter. 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Filter
    {        
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public IEnumerable<IAdjustment> Adjustments { get; set; }
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
