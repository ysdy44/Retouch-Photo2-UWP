using System.Collections.Generic;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Category of <see cref = "Style" />.
    /// </summary>
    public class StyleCategory
    {
        /// <summary> Gets or sets the name. </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The source data.
        /// </summary>
        public IList<Style> Styles { get; set; } = new List<Style>();
    }
}