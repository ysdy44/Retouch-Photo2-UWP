using FanKit.Transformers;
using Microsoft.Graphics.Canvas;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        public NodeCollection Nodes { get; protected set; } = null;

        /// <summary>
        /// Convert to curves layer.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product nodes. </returns>
        public abstract NodeCollection ConvertToCurves(ICanvasResourceCreator resourceCreator);

    }
}