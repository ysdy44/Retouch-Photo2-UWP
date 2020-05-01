using Retouch_Photo2.Brushs.Models;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Mode of <see cref="Brush"> operate.
    /// </summary>
    public enum BrushOperateMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Operate <see cref="LinearGradientBrush.StartPoint"/>. </summary>
        LinearStartPoint,
        /// <summary> Operate <see cref="LinearGradientBrush.EndPoint"/>. </summary>
        LinearEndPoint,

        /// <summary> Operate <see cref="RadialGradientBrush.Center"/>. </summary>
        RadialCenter,
        /// <summary> Operate <see cref="RadialGradientBrush.Point"/>. </summary>
        RadialPoint,

        /// <summary> Operate <see cref="EllipticalGradientBrush.Center"/>. </summary>
        EllipticalCenter,
        /// <summary> Operate <see cref="EllipticalGradientBrush.XPoint"/>. </summary>
        EllipticalXPoint,
        /// <summary> Operate <see cref="EllipticalGradientBrush.YPoint"/>. </summary>
        EllipticalYPoint,
    }
}