namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Type of <see cref="IBrush"/>.
    /// </summary>
    public enum BrushType
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Soild color brush. </summary>
        Color,

        /// <summary> Linear gradien brush. </summary>
        LinearGradient,
        /// <summary> Radial gradien brush. </summary>
        RadialGradient,
        /// <summary> Elliptical gradien brush. </summary>
        EllipticalGradient,

        /// <summary> Image brush. </summary>
        Image,
    }
}