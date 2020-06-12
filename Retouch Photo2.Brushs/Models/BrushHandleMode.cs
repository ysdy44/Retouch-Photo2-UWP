namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Handle-mode of <see cref="BrushBase"/>.
    /// </summary>
    public enum BrushHandleMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> <see cref="IBrush.Center"/>. </summary>
        Center,
        /// <summary> <see cref="IBrush.XPoint"/>. </summary>
        XPoint,
        /// <summary> <see cref="IBrush.YPoint"/>. </summary>
        YPoint,
    }
}