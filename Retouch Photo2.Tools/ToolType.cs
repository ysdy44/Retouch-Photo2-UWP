namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Type of <see cref = "Tool" />.
    /// </summary>
    public enum ToolType
    {
        /// <summary> Normal </summary>
        None,

        /// <summary> Drag and move the canvas. </summary>
        View,

        /// <summary> Rectangle Geometry </summary>
        Rectangle,

        /// <summary> Ellipse Geometry </summary>
        Ellipse,

        /// <summary> Cursor Tool</summary>
        Cursor,
    }
}