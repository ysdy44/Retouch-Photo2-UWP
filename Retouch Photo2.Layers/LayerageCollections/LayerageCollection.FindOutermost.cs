using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        /// <summary>
        /// Find outermost layerage
        /// in all selected layers.
        /// </summary>
        public static Layerage FindOutermost_SelectedLayer(IEnumerable<Layerage> selectedLayerages)
        {
            int index = int.MaxValue;
            Layerage layerage = null;

            foreach (Layerage selecedLayerage in selectedLayerages)
            {
                ILayer selectedLayer = selecedLayerage.Self;

                if (index > selectedLayer.Control.Depth)
                {
                    index = selectedLayer.Control.Depth;
                    layerage = selecedLayerage;
                }
            }

            return layerage;
        }

    }
}
