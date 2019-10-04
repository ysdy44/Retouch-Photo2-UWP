namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Mode of <see cref="Brush"> operate.
    /// </summary>
    public enum BrushOperateMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> The linear-gradient start-point. </summary>
        LinearStartPoint,
        /// <summary> The linear-gradient end-point. </summary>
        LinearEndPoint,

        /// <summary> The radial-gradient center. </summary>
        RadialCenter,
        /// <summary> The radial-gradient point. </summary>
        RadialPoint,

        /// <summary> The elliptical-gradient center. </summary>
        EllipticalCenter,
        /// <summary> The elliptical-gradient X-point. </summary>
        EllipticalXPoint,
        /// <summary> The elliptical-gradient Y-point. </summary>
        EllipticalYPoint,
    }
}