namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Mode of <see cref="Brush"> operate.
    /// </summary>
    public enum BrushOperateMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> LinearGradientStart point. </summary>
        LinearGradientStartPoint,
        /// <summary> LinearGradientEnd point. </summary>
        LinearGradientEndPoint,
        
        /// <summary> RadialGradientCener point. </summary>
        RadialGradientCenter,
        /// <summary> RadialGradientPoint. </summary>
        RadialGradientPoint,
               
        /// <summary> EllipticalGradientCenter point. </summary>
        EllipticalGradientCenter,
        /// <summary> EllipticalGradientX point. </summary>
        EllipticalGradientXPoint,
        /// <summary> EllipticalGradientY point. </summary>
        EllipticalGradientYPoint,
    }
}