using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Gets all selected layers.
        /// </summary>
        /// <returns> The selected layers. </returns>
        public static IEnumerable<ILayer> GetAllSelectedLayers(LayerCollection layerCollection) 
         {
            IList<ILayer> addLayers = new List<ILayer>();

            void addLayer(IList<ILayer> layers)
            {
                foreach (ILayer child in layers)
                {
                    if (child.IsSelected == true)
                    {
                        addLayers.Add(child);
                    }

                    //Recursive
                    addLayer(child.Children);
                }
            }

            //Recursive
            addLayer(layerCollection.RootLayers);

            return addLayers;
        }


        #region ShiftSelect


        /// <summary>
        /// Select the current layer (hold **Shift** at the same time).
        /// </summary>
        /// <param name="currentLayer"> The current layer. </param>
        public static void ShiftSelectCurrentLayer(LayerCollection layerCollection, ILayer currentLayer)
        {
            IList<ILayer> parentsChildren = layerCollection.GetParentsChildren(currentLayer);

            //Recursive
            bool isFind = LayerCollection._findShiftSelectedLayer(currentLayer, parentsChildren);
            if (isFind) LayerCollection._setShiftSelectedLayer(currentLayer, parentsChildren);
        }
        
        private static bool _findShiftSelectedLayer(ILayer currentLayer, IEnumerable<ILayer> layers)
        {
            // Find self layer and selected layer.
            bool existSelectedLayer = false;
            bool existSelfLayer = false;

            foreach (ILayer child in layers)
            {
                if (existSelectedLayer == false)
                    if (child.IsSelected == true)
                        existSelectedLayer = true;

                if (existSelfLayer == false)
                    if (child == currentLayer)
                        existSelfLayer = true;
            }

            return (existSelectedLayer && existSelfLayer);
        }

        private static void _setShiftSelectedLayer(ILayer currentLayer, IEnumerable<ILayer> layers)
        {
            // Find self layer and selected layer.
            bool existSelectedLayer = false;
            bool existSelfLayer = false;

            foreach (ILayer child in layers)
            {
                if (existSelectedLayer && existSelfLayer)
                    break;

                if (existSelectedLayer || existSelfLayer)
                    child.IsSelected = true;


                if (existSelectedLayer == false)
                    if (child.IsSelected == true)
                        existSelectedLayer = true;

                if (existSelfLayer == false)
                    if (child == currentLayer)
                        existSelfLayer = true;
            }

            currentLayer.IsSelected = true;
        }
               

        #endregion
        
    }
}