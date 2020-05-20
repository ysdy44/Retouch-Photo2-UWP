using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Remove a layer.
        /// </summary>      
        public static void RemoveLayer(LayerCollection layerCollection, ILayer removeLayer)
        {
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(removeLayer);

            parentsChildren.Remove(removeLayer);
        }

        /// <summary>
        /// Remove all selected layers.
        /// </summary>
        public static void RemoveAllSelectedLayers(LayerCollection layerCollection) => LayerCollection._removeAllSelectedLayers(layerCollection, layerCollection.RootLayers);


        private static void _removeAllSelectedLayers(LayerCollection layerCollection, IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                //Recursive
                if (child.IsSelected == true)
                    LayerCollection._removeAllLayers(layerCollection, child.Children);
                //Recursive
                else
                    LayerCollection._removeAllSelectedLayers(layerCollection, child.Children);
            }

            //Remove
            ILayer removeLayer = null;
            do
            {
                layers.Remove(removeLayer);

                removeLayer = layers.FirstOrDefault(layer => layer.IsSelected == true);
            }
            while (removeLayer != null);
        }

        private static void _removeAllLayers(LayerCollection layerCollection, IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                //Recursive
                LayerCollection._removeAllLayers(layerCollection, child.Children);

                layerCollection.RootControls.Remove(child.Control.Self);
            }
            layers.Clear();
        }
        

    }
}