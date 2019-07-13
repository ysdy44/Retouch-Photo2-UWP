namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Type of <see cref = "Tool" />.
    /// </summary>
    public enum ToolType
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Cursor tool. </summary>
        Cursor,

        /// <summary> Drag and move the canvas. </summary>
        View,

        /// <summary> Fill or stroke geometry. </summary>
        Brush,

        /// <summary> Create rectangle geometry. </summary>
        Rectangle,

        /// <summary> Create ellipse geometry. </summary>
        Ellipse,

        /// <summary> Pen curve tool. </summary>
        Pen,

        /// <summary> Create image layer. </summary>
        Image,

        /// <summary> Create acrylic layer. </summary>
        Acrylic
    }
}