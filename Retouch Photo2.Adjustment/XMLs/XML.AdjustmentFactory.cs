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
            IAdjustment adjustment = XML._createAdjustment(type);
            adjustment.Load(element);
            return adjustment;
        }
        private static IAdjustment _createAdjustment(string type)
        {
            switch (type)
            {
                case "Gray": return new GrayAdjustment();
                case "Invert": return new InvertAdjustment();
                case "Exposure": return new ExposureAdjustment();
                case "Brightness": return new BrightnessAdjustment();
                case "Saturation": return new SaturationAdjustment();
                case "HueRotation": return new HueRotationAdjustment();
                case "Contrast": return new ContrastAdjustment();
                case "Temperature": return new TemperatureAdjustment();
                case "HighlightsAndShadows": return new HighlightsAndShadowsAdjustment();
                case "GammaTransfer": return new GammaTransferAdjustment();
                case "Vignette": return new VignetteAdjustment();
                default: return new GrayAdjustment();
            }
        }

    }
}