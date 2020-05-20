using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Find outermost layer
        /// in all selected layers.
        /// </summary>
        public static ILayer FindOutermost_SelectedLayer(IEnumerable<ILayer> selecedLayers)
        {
            int index = int.MaxValue;
            ILayer layer = null;

            foreach (ILayer selecedLayer in selecedLayers)
            {
                if (index > selecedLayer.Control.Depth)
                {
                    index = selecedLayer.Control.Depth;
                    layer = selecedLayer;
                }
            }

            return layer;
        }
               
    }
}
