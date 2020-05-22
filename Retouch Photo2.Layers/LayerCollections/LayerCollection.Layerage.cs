using Retouch_Photo2.Brushs;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Gets all layerages
        /// </summary>
        /// <returns> The yield layerages. </returns>
        public static IEnumerable<Layerage> GetLayerages(IEnumerable<Layerage> layers)
        {
            foreach (Layerage child in layers)
            {
                yield return child;

                foreach (Layerage photocopier in LayerCollection.GetLayerages(child.Children))
                {
                    yield return photocopier;
                }
            }
        }

    }
}