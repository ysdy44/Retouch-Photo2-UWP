using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Remove a layerage.
        /// </summary>      
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <param name="removeLayerage"> The remove Layerage. </param>
        public static void Remove(LayerageCollection layerageCollection, Layerage removeLayerage)
        {
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(removeLayerage);

            parentsChildren.Remove(removeLayerage);
        }

        /// <summary>
        /// Remove all selected layerages.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        public static void RemoveAllSelected(LayerageCollection layerageCollection) => LayerageCollection._removeAllSelected(layerageCollection, layerageCollection.RootLayerages);


        private static void _removeAllSelected(LayerageCollection layerageCollection, IList<Layerage> layerages)
        {        
            foreach (Layerage child in layerages)
            {
                ILayer layer = child.Self;

                //Recursive
                if (layer.IsSelected == true)
                    LayerageCollection._removeAll(layerageCollection, child.Children);
                //Recursive
                else
                    LayerageCollection._removeAllSelected(layerageCollection, child.Children);
            }

            //Remove
            Layerage removeLayerage = null;
            do
            {
                layerages.Remove(removeLayerage);

                removeLayerage = layerages.FirstOrDefault(layerage => layerage.Self.IsSelected == true);
            }
            while (removeLayerage != null);
        }

        private static void _removeAll(LayerageCollection layerageCollection, IList<Layerage> layerages)
        {         
            foreach (Layerage child in layerages)
            {
                ILayer child2 = child.Self;

                //Recursive
                LayerageCollection._removeAll(layerageCollection, child.Children);

                layerageCollection.RootControls.Remove(child2.Control.Self);
            }
            layerages.Clear();
        }


    }
}