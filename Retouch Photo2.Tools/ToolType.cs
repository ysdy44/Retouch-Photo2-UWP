namespace Retouch_Photo2.Tools
{
    /// <summary> 
    /// Type of Tool.
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
        /// <summary> Transparency tool. </summary>
        Transparency,

        //Geometry0
        /// <summary> Create rectangle geometry. </summary>
        GeometryRectangle,
        /// <summary> Create ellipse geometry. </summary>
        GeometryEllipse,
        /// <summary> Pen curve tool. </summary>
        Pen,

        /// <summary> Create image layer. </summary>
        Image,
        /// <summary> Create acrylic layer. </summary>
        Acrylic,
        /// <summary> Vector crop tool. </summary>
        Crop,


        //Geometry1
        /// <summary> 圆角矩形. </summary>
        GeometryRoundRect,
        /// <summary> 三角形. </summary>
        GeometryTriangle,
        /// <summary> 菱形. </summary>
        GeometryDiamond,

        //Geometry2
        /// <summary> 多边形. </summary>
        GeometryPentagon,
        /// <summary> 星星. </summary>
        GeometryStar,
        /// <summary> 齿轮. </summary>
        GeometryCog,

        //Geometry3
        /// <summary> 甜甜圈. </summary>
        GeometryDount,
        /// <summary> 饼图. </summary>
        GeometryPie,
        /// <summary> 曲奇饼. </summary>
        GeometryCookie,

        //Geometry4
        /// <summary> 箭头. </summary>
        GeometryArrow,
        /// <summary> 胶囊. </summary>
        GeometryCapsule,
        /// <summary> 心型. </summary>
        GeometryHeart,

    }
}