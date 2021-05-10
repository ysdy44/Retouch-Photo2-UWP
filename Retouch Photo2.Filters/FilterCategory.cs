// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections.Generic;

namespace Retouch_Photo2.Filters
{
    /// <summary>
    /// Category of <see cref = "Filter" />.
    /// </summary>
    public class FilterCategory
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The source data.
        /// </summary>
        public IEnumerable<Filter> Filters { get; set; }
    }
}