namespace Retouch_Photo2.Controls
{
    /// <summary>
    /// State of <see cref="LayerControl"/>. 
    /// </summary>
    public enum LayerControlState
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Disable. </summary>
        Disable,

        /// <summary> Change Layer property (the layer with children). </summary>
        SingleLayerWithChildren,
        /// <summary> Change Layer property (the layer without children). </summary>
        SingleLayerWithoutChildren,
        /// <summary> Change Layers property. </summary>
        MultipleLayer,

        /// <summary> Blends. </summary>
        Blends,
        /// <summary> Children. </summary>
        Children
    }
}