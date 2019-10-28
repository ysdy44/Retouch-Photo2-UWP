using Retouch_Photo2.Adjustments.Models;
using System.Xml.Linq;

namespace Retouch_Photo2.Adjustments
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a IAdjustment from the string.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The created IAdjustment. </returns>
        public static IAdjustment CreateAdjustment(XElement element)
        {
            string type = element.Name.LocalName;

            switch (type)
            {
                case "Gray": return new GrayAdjustment();
                case "Invert": return new InvertAdjustment();
                case "Exposure": return new ExposureAdjustment(element);
                case "Brightness": return new BrightnessAdjustment(element);
                case "Saturation": return new SaturationAdjustment(element);
                case "HueRotation": return new HueRotationAdjustment(element);
                case "Contrast": return new ContrastAdjustment(element);
                case "Temperature": return new TemperatureAdjustment(element);
                case "HighlightsAndShadows": return new HighlightsAndShadowsAdjustment(element);
                case "GammaTransfer": return new GammaTransferAdjustment(element);
                case "Vignette": return new VignetteAdjustment(element);
                default: return new GrayAdjustment();
            }
        }

    }
}