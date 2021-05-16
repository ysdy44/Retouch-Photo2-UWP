// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using System.Collections.Generic;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a ITool from an string and XElement.
        /// </summary>
        /// <param name="name"> The source name. </param>
        /// <param name="strings"> The source strings. </param>
        /// <param name="language"> The destination language. </param>
        /// <returns> The created string. </returns>
        public static string CreateString(string name, IDictionary<string, string> strings, string language)
        {
            if (strings is null) return name;

            if (strings.ContainsKey(language)) return strings[language];

            foreach (var item in strings)
            {
                if (language.Contains(item.Key))
                {
                    return item.Value;
                }
            }

            if (name is null) return string.Empty;

            return name;
        }

    }
}