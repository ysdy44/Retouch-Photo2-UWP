namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// Type of Layer.
    /// </summary>
    public enum LayerType
    {
        /// <summary> Normal. </summary>
        None,

        //Geometry0            
        /// <summary> Geometry rectangle. </summary>                           
        GeometryRectangle,
        /// <summary> Geometry ellipse. </summary>
        GeometryEllipse,
        /// <summary> Geometry curve. </summary>
        GeometryCurve,

        /// <summary> Image. </summary>
        Image,
        /// <summary> Acrylic. </summary>
        Acrylic,
        /// <summary> Group. </summary>
        Group,

        //Geometry1
        /// <summary> Geometry round-rect. </summary>
        GeometryRoundRect,
        /// <summary> Geometry triangle. </summary>
        GeometryTriangle,
        /// <summary> Geometry diamond. </summary>
        GeometryDiamond,

        //Geometry2
        /// <summary> Geometry pentagon. </summary>
        GeometryPentagon,
        /// <summary> Geometry star. </summary>
        GeometryStar,
        /// <summary> Geometry cog. </summary>
        GeometryCog,

        //Geometry3
        /// <summary> Geometry dount. </summary>
        GeometryDount,
        /// <summary> Geometry pie. </summary>
        GeometryPie,
        /// <summary> Geometry cookie. </summary>
        GeometryCookie,

        //Geometry4
        /// <summary> Geometry arrow. </summary>
        GeometryArrow,
        /// <summary> Geometry capsule. </summary>
        GeometryCapsule,
        /// <summary> Geometry heart,. </summary>
        GeometryHeart,
    }
}