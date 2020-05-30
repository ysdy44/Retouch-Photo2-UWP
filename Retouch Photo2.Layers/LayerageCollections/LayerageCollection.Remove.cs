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
        /// Remove a layer.
        /// </summary>      
        public static void RemoveLayer(LayerageCollection layerageCollection, Layerage removeLayerage)
        {
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(removeLayerage);

            parentsChildren.Remove(removeLayerage);
        }

        /// <summary>
        /// Remove all selected layers.
        /// </summary>
        public static void RemoveAllSelectedLayers(LayerageCollection layerageCollection) => LayerageCollection._removeAllSelectedLayers(layerageCollection, layerageCollection.RootLayerages);


        private static void _removeAllSelectedLayers(LayerageCollection layerageCollection, IList<Layerage> layerages)
        {        
            foreach (Layerage child in layerages)
            {
                ILayer layer = child.Self;

                //Recursive
                if (layer.IsSelected == true)
                    LayerageCollection._removeAllLayers(layerageCollection, child.Children);
                //Recursive
                else
                    LayerageCollection._removeAllSelectedLayers(layerageCollection, child.Children);
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

        private static void _removeAllLayers(LayerageCollection layerageCollection, IList<Layerage> layerages)
        {         
            foreach (Layerage child in layerages)
            {
                ILayer child2 = child.Self;

                //Recursive
                LayerageCollection._removeAllLayers(layerageCollection, child.Children);

                layerageCollection.RootControls.Remove(child2.Control.Self);
            }
            layerages.Clear();
        }


    }
}