namespace Retouch_Photo2.Elements
{
    /// <summary> Mode of composite between layers. </summary>
    public enum CompositeMode
    {
        /// <summary> New layer. </summary>
        New,
        /// <summary> Union of source and destination layers. </summary>
        Add,
        /// <summary> Region of the source layer. </summary>
        Subtract,
        /// <summary> Intersection of source and destination layers. </summary>
        Intersect,

        /// <summary> Union of source and destination layers with xor function for pixels that overlap. </summary>
        Xor,
    }
}