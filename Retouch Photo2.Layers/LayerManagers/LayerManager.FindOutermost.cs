using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Manager of <see cref="ILayer"/>.
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public static partial class LayerManager
    {

        /// <summary>
        /// Find first layerage by <see cref="ILayer.Id"/>.
        /// </summary>
        /// <param name="layer"> The layer. </param>
        /// <returns> The found layerage. </returns>
        public static Layerage FindFirstLayerage(ILayer layer) => LayerManager._findFirstLayerage(LayerManager.Layerage, layer);
        private static Layerage _findFirstLayerage(Layerage layerage, ILayer layer)
        {
            foreach (Layerage child in layerage.Children)
            {
                if (child.Id == layer.Id) return child;


                if (child.Children.Count != 0)
                {
                    Layerage find = LayerManager._findFirstLayerage(child, layer);
                    if (find != null) return find;
                }
            }

            return null;
        }



        /// <summary>
        /// Find outermost layerage  in all selected layers.
        /// </summary>
        /// <returns> The found layerage. </returns>
        public static Layerage FindOutermostLayerage(IEnumerable<Layerage> selectedLayerages)
        {
            int index = int.MaxValue;
            Layerage layerage = null;

            foreach (Layerage selecedLayerage in selectedLayerages)
            {
                ILayer selectedLayer = selecedLayerage.Self;

                if (index > selectedLayer.Control.Depth)
                {
                    index = selectedLayer.Control.Depth;
                    layerage = selecedLayerage;
                }
            }

            return layerage;
        }


    }
}
