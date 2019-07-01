namespace Retouch_Photo2.Adjustments
{
    /// <summary> 
    /// Type of <see cref = "Adjustment"/>.
    /// </summary>
    public enum AdjustmentType
    {
        /// <summary> Gray. </summary>
        Gray,
        /// <summary> Invert. </summary>
        Invert,

        /// <summary> Exposure. </summary>
        Exposure,
        /// <summary> Brightness. </summary>
        Brightness,
        /// <summary> Saturation. </summary>
        Saturation,
        /// <summary> Hue rotation. </summary>
        HueRotation,
        /// <summary> Contrast. </summary>
        Contrast,
        /// <summary> Temperature. </summary>
        Temperature,

        /// <summary> Highlights and shadows. </summary>
        HighlightsAndShadows,
        /// <summary> Gamma transfer. </summary>
        GammaTransfer,
        /// <summary> Vignette. </summary>
        Vignette
    }
}