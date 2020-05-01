using Retouch_Photo2.Brushs.Models;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a Brush from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created Layer. </returns>
        public static IBrush CreateBrush(string type)
        {
            switch (type)
            {
                case "None": return new NoneBrush();
                case "Color": return new ColorBrush();
                case "LinearGradient": return new LinearGradientBrush();
                case "RadialGradient": return new RadialGradientBrush();
                case "EllipticalGradient": return new EllipticalGradientBrush();
                case "Image": return new ImageBrush();

                default: return new NoneBrush();
            }
        }

    }
}