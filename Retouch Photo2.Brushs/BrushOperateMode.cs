using Retouch_Photo2.Brushs.Models;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Mode of <see cref="Brush"> operate.
    /// </summary>
    public enum BrushOperateMode
    {
        /// <summary> Normal. </summary>
        InitializeController,

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


        /// <summary> Translation. </summary>
        ImageTranslation,
        /// <summary> Rotation. </summary>
        ImageRotation,

        /// <summary> Skew (Left). </summary>
        ImageSkewLeft,
        /// <summary> Skew (Top). </summary>
        ImageSkewTop,
        /// <summary> Skew (Right). </summary>
        ImageSkewRight,
        /// <summary> Skew (Bottom). </summary>
        ImageSkewBottom,

        /// <summary> Scale (Left). </summary>
        ImageScaleLeft,
        /// <summary> Scale (Top). </summary>
        ImageScaleTop,
        /// <summary> Scale (Right). </summary>
        ImageScaleRight,
        /// <summary> Scale (Bottom). </summary>
        ImageScaleBottom,

        /// <summary> Scale (LeftTop). </summary>
        ImageScaleLeftTop,
        /// <summary> Scale (RightTop). </summary>
        ImageScaleRightTop,
        /// <summary> Scale (RightBottom). </summary>
        ImageScaleRightBottom,
        /// <summary> Scale (LeftBottom). </summary>
        ImageScaleLeftBottom,
    }
}