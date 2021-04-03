// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         
// Only:              ★★★
// Complete:      ★
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Category of <see cref = "IStyle" />.
    /// </summary>
    public class StyleCategory : IGrouping<string, IStyle>
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; } = string.Empty;
        public string Key => this.Name;

        /// <summary>
        /// The source data.
        /// </summary>
        public IList<IStyle> Styles { get; set; } = new List<IStyle>();
        public IEnumerator<IStyle> GetEnumerator() => this.Styles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.Styles.GetEnumerator();
    }
}