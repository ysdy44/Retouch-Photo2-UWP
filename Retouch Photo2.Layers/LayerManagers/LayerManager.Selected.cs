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
        /// Gets all selected layerages.
        /// </summary>
        /// <returns> The selected layerages. </returns>
        public static IEnumerable<Layerage> GetAllSelected()
        {
            IList<Layerage> selectedLayerages = new List<Layerage>();
   
            void addLayer(Layerage layerage)
            {
                foreach (Layerage child in layerage.Children) 
                {
                    ILayer layer = child.Self;

                    if (layer.IsSelected == true)
                    {
                        selectedLayerages.Add(child);
                    }
                    else
                    {
                        // Recursive
                        addLayer(child);
                    }
                }
            }

            // Recursive
            addLayer(LayerManager.RootLayerage);

            return selectedLayerages;
        }

        /// <summary>
        /// Gets all selected layerages.
        /// </summary>
        /// <returns> The selected layers. </returns>
        public static IEnumerable<Layerage> GetAllSelectedRecursive()
        {
            IList<Layerage> selectedLayerages = new List<Layerage>();

            void addLayer(Layerage layerage)
            {
                foreach (Layerage child in layerage.Children)
                {
                    ILayer layer = child.Self;

                    if (layer.IsSelected == true)
                    {
                        selectedLayerages.Add(child);
                    }

                    // Recursive
                    addLayer(child);
                }
            }

            // Recursive
            addLayer(LayerManager.RootLayerage);

            return selectedLayerages;
        }

        
    }
}