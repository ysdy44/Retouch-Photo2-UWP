// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Filters
{
    /// <summary>
    /// Category of <see cref = "Filter" />.
    /// </summary>
    public class FilterCategory : IGrouping<string, Filter>
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; } = string.Empty;
        public string Key => this.Name;

        /// <summary>
        /// The source data.
        /// </summary>
        public IList<Filter> Filters { get; set; } = new List<Filter>();
        public IEnumerator<Filter> GetEnumerator() => this.Filters.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Filters.GetEnumerator();
    }
}