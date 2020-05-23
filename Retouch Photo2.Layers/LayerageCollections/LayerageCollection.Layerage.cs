using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {


        /// <summary>
        /// Initializes the layerage's control.
        /// </summary>
        /// <param name="layerages"></param>
        public static void ConstructLayerControl(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage child in layerages)
            {
                ILayer child2 = child.Self;

                child2.Control.ConstructLayerControl(child);

                LayerageCollection.ConstructLayerControl(child.Children);
            }
        }


        /// <summary>
        /// Gets all layerages
        /// </summary>
        /// <returns> The yield layerages. </returns>
        public static IEnumerable<Layerage> GetLayerages(IEnumerable<Layerage> layerages)
        {
            foreach (Layerage child in layerages)
            {
                yield return child;

                foreach (Layerage photocopier in LayerageCollection.GetLayerages(child.Children))
                {
                    yield return photocopier;
                }
            }
        }

    }
}