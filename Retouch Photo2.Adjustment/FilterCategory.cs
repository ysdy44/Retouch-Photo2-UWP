using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// <see cref = "Filter" />'s category. 
    /// </summary>
    public class FilterCategory
    {
        public string Name { get; set; } = string.Empty;

        public List<Filter> Filters { get; set; } = new List<Filter>();
    }
}