namespace Retouch_Photo2.Library
{
    /// <summary> <see cref = "TransformerMode" />'s mode. </summary>
    public enum TransformerMode
    { 
        /// <summary> Normal. </summary>
        None,
        /// <summary> Translation. </summary>
        Translation,
        /// <summary> Rotation. </summary>
        Rotation,

        /// <summary> Skew (Left). </summary>
        SkewLeft,
        /// <summary> Skew (Top). </summary>
        SkewTop,
        /// <summary> Skew (Right). </summary>
        SkewRight,
        /// <summary> Skew (Bottom). </summary>
        SkewBottom,

        /// <summary> Scale (Left). </summary>
        ScaleLeft,
        /// <summary> Scale (Top). </summary>
        ScaleTop,
        /// <summary> Scale (Right). </summary>
        ScaleRight,
        /// <summary> Scale (Bottom). </summary>
        ScaleBottom,

        /// <summary> Scale (LeftTop). </summary>
        ScaleLeftTop,
        /// <summary> Scale (RightTop). </summary>
        ScaleRightTop,
        /// <summary> Scale (RightBottom). </summary>
        ScaleRightBottom,
        /// <summary> Scale (LeftBottom). </summary>
        ScaleLeftBottom,
    }
}