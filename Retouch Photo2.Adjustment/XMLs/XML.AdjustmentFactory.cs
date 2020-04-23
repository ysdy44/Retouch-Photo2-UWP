using Retouch_Photo2.Adjustments.Models;

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
       /// <param name="type"> The source string. </param>
        /// <returns> The created IAdjustment. </returns>
        private static IAdjustment CreateAdjustment(string type)
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