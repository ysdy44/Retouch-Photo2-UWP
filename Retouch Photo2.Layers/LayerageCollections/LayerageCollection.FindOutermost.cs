using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Find first layerage by <see cref="ILayer.Id">.
        /// </summary>
        public Layerage FindFirstLayerage(ILayer layer) => this._findFirstLayerage(this.RootLayerages, layer);
        private Layerage _findFirstLayerage(IEnumerable<Layerage> layerages, ILayer layer)
        {
            foreach (Layerage child in layerages)
            {
                if (child.Id == layer.Id) return child;


                if (child.Children.Count != 0)
                {
                    Layerage find = this._findFirstLayerage(child.Children, layer);
                    if (find != null) return find;
                }
            }

            return null;
        }



        /// <summary>
        /// Find outermost layerage
        /// in all selected layers.
        /// </summary>
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
