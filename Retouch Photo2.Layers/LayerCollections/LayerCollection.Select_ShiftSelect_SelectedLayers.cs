using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Gets all selected layers.
        /// </summary>
        /// <returns> The selected layers. </returns>
        public static IEnumerable<Layerage> GetAllSelectedLayers(LayerCollection layerCollection) 
         {
            IList<Layerage> addLayers = new List<Layerage>();

            void addLayer(IList<Layerage> layers)
            {
                foreach (Layerage child in layers)
                {
                    ILayer child2 = child.Self;

                    if (child2.IsSelected == true)
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
        public static void ShiftSelectCurrentLayer(LayerCollection layerCollection, Layerage currentLayer)
        {
            IList<Layerage> parentsChildren = layerCollection.GetParentsChildren(currentLayer);

            //Recursive
            bool isFind = LayerCollection._findShiftSelectedLayer(currentLayer, parentsChildren);
            if (isFind) LayerCollection._setShiftSelectedLayer(currentLayer, parentsChildren);
        }
        
        private static bool _findShiftSelectedLayer(Layerage currentLayer, IEnumerable<Layerage> layers)
        {
            // Find self layer and selected layer.
            bool existSelectedILayer = false;
            bool existSelfILayer = false;

            foreach (Layerage child in layers)
            {
                ILayer child2 = child.Self;

                if (existSelectedILayer == false)
                    if (child2.IsSelected == true)
                        existSelectedILayer = true;

                if (existSelfILayer == false)
                    if (child == currentLayer)
                        existSelfILayer = true;
            }

            return (existSelectedILayer && existSelfILayer);
        }

        private static void _setShiftSelectedLayer(Layerage currentLayer, IEnumerable<Layerage> layers)
        {
            ILayer currentLayer2 = currentLayer.Self;

            // Find self layer and selected layer.
            bool existSelectedILayer = false;
            bool existSelfILayer = false;

            foreach (Layerage child in layers)
            {
                ILayer child2 = child.Self;
                if (existSelectedILayer && existSelfILayer)
                    break;

                if (existSelectedILayer || existSelfILayer)
                    child2.IsSelected = true;


                if (existSelectedILayer == false)
                    if (child2.IsSelected == true)
                        existSelectedILayer = true;

                if (existSelfILayer == false)
                    if (child == currentLayer)
                        existSelfILayer = true;
            }

            currentLayer2.IsSelected = true;
        }
               

        #endregion
        
    }
}