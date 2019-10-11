namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Type of Touchbar.
    /// </summary>
    public enum TouchbarType
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Geometry stroke-width. </summary>
        StrokeWidth,

        /// <summary> ViewTool's radian. </summary>
        ViewRadian,
        /// <summary> ViewTool's scale. </summary>
        ViewScale,

        /// <summary> AcrylicTool's tint-opacity. </summary>
        AcrylicTintOpacity,
        /// <summary> AcrylicTool's tint-amount. </summary>
        AcrylicBlurAmount,

        /// <summary> GeometryRoundRectTool's corner. </summary>
        GeometryRoundRectCorner,

        /// <summary> GeometryTriangleTool's center-point. </summary>
        GeometryTriangleCenter,

        /// <summary> GeometryPentagonTool's points. </summary>
        GeometryPentagonPoints,

        /// <summary> GeometryStarTool's points. </summary>
        GeometryStarPoints,
        /// <summary> GeometryStarTool's inner-radius . </summary>
        GeometryStarInnerRadius,

    }
}