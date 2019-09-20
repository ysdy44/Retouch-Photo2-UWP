using Newtonsoft.Json;
using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Provides adjustments for filter.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public interface IFilter2222222222222
    {
        [JsonProperty]
        string Name { get; set; }

        [JsonProperty]
        IEnumerable<IAdjustment> Adjustments { get; set; }
    }
}