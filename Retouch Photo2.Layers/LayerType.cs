namespace Retouch_Photo2.Layers
{
    /// <summary> 
    /// Type of <see cref="ILayer"/>.
    /// </summary>
    public enum LayerType
    {
        /// <summary> Normal. </summary>
        None,

        //Geometry0            
        /// <summary> Rectangle geometry. </summary>                           
        GeometryRectangle,
        /// <summary> Rllipse geometry. </summary>
        GeometryEllipse,

        /// <summary> Curve. </summary>
        Curve,

        /// <summary> Frame Text. </summary>
        TextFrame,
        /// <summary> Artistic Text. </summary>
        TextArtistic,

        /// <summary> Image. </summary>
        Image,
        /// <summary> Group. </summary>
        Group,

        /// <summary> Combine-add. </summary>
        CombineAdd,

        /// <summary> Grid pattern. </summary>
        PatternGrid,

        //Geometry1
        /// <summary> Round-rect geometry. </summary>
        GeometryRoundRect,
        /// <summary> Triangle geometry. </summary>
        GeometryTriangle,
        /// <summary> Diamond geometry. </summary>
        GeometryDiamond,

        //Geometry2
        /// <summary> Pentagon geometry. </summary>
        GeometryPentagon,
        /// <summary> Star geometry. </summary>
        GeometryStar,
        /// <summary> Cog geometry. </summary>
        GeometryCog,

        //Geometry3
        /// <summary> Dount geometry. </summary>
        GeometryDount,
        /// <summary> Pie geometry. </summary>
        GeometryPie,
        /// <summary> Cookie geometry. </summary>
        GeometryCookie,

        //Geometry4
        /// <summary> Arrow geometry. </summary>
        GeometryArrow,
        /// <summary> Capsule geometry. </summary>
        GeometryCapsule,
        /// <summary> Heart geometry,. </summary>
        GeometryHeart,
    }
}