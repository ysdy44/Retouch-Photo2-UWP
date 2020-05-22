using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Remove a layer.
        /// </summary>      
        public static void RemoveLayer(LayerCollection layerCollection, Layerage removeLayer)
        {
            IList<Layerage> parentsChildren = layerCollection.GetParentsChildren(removeLayer);

            parentsChildren.Remove(removeLayer);
        }

        /// <summary>
        /// Remove all selected layers.
        /// </summary>
        public static void RemoveAllSelectedLayers(LayerCollection layerCollection) => LayerCollection._removeAllSelectedLayers(layerCollection, layerCollection.RootLayers);


        private static void _removeAllSelectedLayers(LayerCollection layerCollection, IList<Layerage> layerages)
        {        
            foreach (Layerage child in layerages)
            {
                ILayer layer = child.Self;

                //Recursive
                if (layer.IsSelected == true)
                    LayerCollection._removeAllLayers(layerCollection, child.Children);
                //Recursive
                else
                    LayerCollection._removeAllSelectedLayers(layerCollection, child.Children);
            }

            //Remove
            Layerage removeLayer = null;
            do
            {
                layerages.Remove(removeLayer);

                removeLayer = layerages.FirstOrDefault(layerage => layerage.Self.IsSelected == true);
            }
            while (removeLayer != null);
        }

        private static void _removeAllLayers(LayerCollection layerCollection, IList<Layerage> layerages)
        {         
            foreach (Layerage child in layerages)
            {
                ILayer child2 = child.Self;

                //Recursive
                LayerCollection._removeAllLayers(layerCollection, child.Children);

                layerCollection.RootControls.Remove(child2.Control.Self);
            }
            layerages.Clear();
        }


    }
}