using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a collection of layers, including a sorting algorithm for layers
    /// </summary>
    public partial class LayerageCollection
    {

        /// <summary>
        /// Gets all selected layerages.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <returns> The selected layerages. </returns>
        public static IEnumerable<Layerage> GetAllSelected(LayerageCollection layerageCollection) 
         {
            IList<Layerage> selectedLayerages = new List<Layerage>();
   
            void addLayer(IList<Layerage> layerages)
            {
                foreach (Layerage layerage in layerages)
                {
                    ILayer layer = layerage.Self;

                    if (layer.IsSelected == true)
                    {
                        selectedLayerages.Add(layerage);
                    }
                    else
                    {
                        //Recursive
                        addLayer(layerage.Children);
                    }
                }
            }

            //Recursive
            addLayer(layerageCollection.RootLayerages);

            return selectedLayerages;
        }

        /// <summary>
        /// Gets all selected layerages.
        /// </summary>
        /// <param name="layerageCollection"> The layerage-collection. </param>
        /// <returns> The selected layers. </returns>
        public static IEnumerable<Layerage> GetAllSelectedRecursive(LayerageCollection layerageCollection)
        {
            IList<Layerage> selectedLayerages = new List<Layerage>();

            void addLayer(IList<Layerage> layerages)
            {
                foreach (Layerage layerage in layerages)
                {
                    ILayer layer = layerage.Self;

                    if (layer.IsSelected == true)
                    {
                        selectedLayerages.Add(layerage);
                    }

                    //Recursive
                    addLayer(layerage.Children);
                }
            }

            //Recursive
            addLayer(layerageCollection.RootLayerages);

            return selectedLayerages;
        }


        #region ShiftSelect
        /*

        /// <summary>
        /// Select the current layer (hold **Shift** at the same time).
        /// </summary>
        /// <param name="currentLayer"> The current layer. </param>
        public static void ShiftSelectCurrentLayer(LayerageCollection layerageCollection, Layerage currentLayer)
        {
            IList<Layerage> parentsChildren = layerageCollection.GetParentsChildren(currentLayer);

            //Recursive
            bool isFind = LayerageCollection._findShiftSelectedLayer(currentLayer, parentsChildren);
            if (isFind) LayerageCollection._setShiftSelectedLayer(currentLayer, parentsChildren);
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
               

         */
        #endregion
        
    }
}