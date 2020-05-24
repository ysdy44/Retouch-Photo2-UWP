using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerageCollection
    {

        /// <summary>
        /// Find layerage 
        /// by ILayer.
        /// in all selected layers.
        /// </summary>
        public Layerage FindLayerage_ByILayer(ILayer layer) => this._findLayerage_ByILayer(this.RootLayerages, layer);
        private Layerage _findLayerage_ByILayer(IEnumerable<Layerage> layerages, ILayer layer)
        {
            foreach (Layerage child in layerages)
            {
                if (child.Id == layer.Id) return child;


                if (child.Children.Count != 0)
                {
                    Layerage find = this._findLayerage_ByILayer(child.Children, layer);
                    if (find != null) return find;
                }
            }

            return null;
        }



        /// <summary>
        /// Find outermost layerage
        /// in all selected layers.
        /// </summary>
        public static Layerage FindOutermost_FromLayerages(IEnumerable<Layerage> selectedLayerages)
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
