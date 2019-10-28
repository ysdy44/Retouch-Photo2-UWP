using System.Collections.Generic;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// <see cref = "IAdjustment" />'s filter. 
    /// </summary>
    public class Filter
    {        
        public string Name { get; set; }

        public IEnumerable<IAdjustment> Adjustments { get; set; }
    }
}