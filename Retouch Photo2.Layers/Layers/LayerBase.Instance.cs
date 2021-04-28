using System;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        //@Static
        /// <summary> Dictionary <see cref="Layerage"/>s instances. </summary>
        public static readonly Dictionary<string, ILayer> Instances = new Dictionary<string, ILayer>();


        /// <summary>
        /// Find the first <see cref="LayerBase"/> by <see cref="Layerage"/>.
        /// </summary>
        /// <param name="layerage"> The source layerage</param>
        /// <returns> The product layer. </returns>
        public static ILayer FindFirstLayer(Layerage layerage)
        {
            string id = layerage.Id;
            return LayerBase.Instances[id];
        }


        /// <summary> ID </summary>
        public string Id { get; set; }


        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="Layerage"/> is equal to this <see cref="LayerBase"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="Layerage"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="Layerage"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(Layerage other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }
        /// <summary>
        /// Returns a boolean indicating whether the given <see cref="LayerBase"/> is equal to this <see cref="LayerBase"/> instance.
        /// </summary>
        /// <param name="other"> The <see cref="LayerBase"/> to compare this instance to. </param>
        /// <returns> True if the other <see cref="LayerBase"/> is equal to this instance; False otherwise. </returns>
        public bool Equals(LayerBase other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }

    }
}