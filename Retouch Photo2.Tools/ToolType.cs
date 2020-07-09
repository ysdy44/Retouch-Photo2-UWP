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
        /// <summary> Node curve tool. </summary>
        Node,

        /// <summary> Create frame text. </summary>
        TextFrame,
        /// <summary> Create artistic text. </summary>
        TextArtistic,

        /// <summary> Create image layer. </summary>
        Image,
        /// <summary> Vector crop tool. </summary>
        Crop,


        /// <summary> More second geometry. </summary>
        More,


        //Pattern
        /// <summary> Grid pattern. </summary>
        PatternGrid,
        /// <summary> Diagonal pattern. </summary>
        PatternDiagonal,
        /// <summary> Spotted pattern. </summary>
        PatternSpotted,


        //Geometry1
        /// <summary> Create round-rect geometry. </summary>
        GeometryRoundRect,
        /// <summary> Create triangle geometry. </summary>
        GeometryTriangle,
        /// <summary> Create diamond geometry. </summary>
        GeometryDiamond,

        //Geometry2
        /// <summary> Create pentagon geometry. </summary>
        GeometryPentagon,
        /// <summary> Create star geometry. </summary>
        GeometryStar,
        /// <summary> Create cog geometry. </summary>
        GeometryCog,

        //Geometry3
        /// <summary> Create dount geometry. </summary>
        GeometryDount,
        /// <summary> Create pie geometry. </summary>
        GeometryPie,
        /// <summary> Create cookie geometry. </summary>
        GeometryCookie,

        //Geometry4
        /// <summary> Create arrow geometry. </summary>
        GeometryArrow,
        /// <summary> Create caopsule geometry. </summary>
        GeometryCapsule,
        /// <summary> Create heart geometry. </summary>
        GeometryHeart,

    }
}