using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a Visibility from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created Visibility. </returns>
        public static Visibility CreateVisibility(string type)
        {
            switch (type)
            {
                case "Collapsed": return Visibility.Collapsed;
                default: return Visibility.Visible;
            }
        }

    }
}