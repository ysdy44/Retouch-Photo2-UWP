// Core:              ★★★
// Referenced:   ★★★
// Difficult:         
// Only:              
// Complete:      ★
using Windows.UI.Xaml;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a <see cref="ElementTheme"/> from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="ElementTheme"/>. </returns>
        public static ElementTheme CreateTheme(string type)
        {
            switch (type)
            {
                case "Default": return ElementTheme.Default;
                case "Light": return ElementTheme.Light;
                case "Dark": return ElementTheme.Dark;

                default: return ElementTheme.Default;
            }
        }

    }
}