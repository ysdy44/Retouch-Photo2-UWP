using System.Collections.Generic;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// <see cref = "Style" />'s category. 
    /// </summary>
    public class StyleCategory
    {
        public string Name { get; set; } = string.Empty;

        public IList<Style> Styles { get; set; } = new List<Style>();
    }
}