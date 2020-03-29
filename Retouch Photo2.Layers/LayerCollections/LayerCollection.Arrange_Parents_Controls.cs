using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Arrange parent-layer for root-layers.
        /// </summary>
        public void ArrangeLayersParents() => LayerCollection._arrangeLayersParents(this.RootLayers);
               
        private static void _arrangeLayersParents(IList<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }
        private static void _arrangeLayersParents(ILayer parents, IList<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                layer.Parents = parents;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }


        /// <summary>
        /// Clear and arrange all root-controls by root-layers.
        /// (Call it when root-layers is cleared or added.)
        /// </summary>
        public void ArrangeLayersControlsWithClearAndAdd()
        {
            this.RootControls.Clear();

            foreach (ILayer child in this.RootLayers)
            {
                this._arrangeLayersControlsWithClearAndAdd(child, 0);
            }
        }
        private void _arrangeLayersControlsWithClearAndAdd(ILayer layer, int depth)
        {
            this.RootControls.Add(layer.Control.Self);
            
            foreach (ILayer child in layer.Children)
            {
                //Recursive
                this._arrangeLayersControlsWithClearAndAdd(child, depth + 1);
            }
        }
        
    }
}