using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        
        /// <summary>
        /// 
        /// </summary>
        public void ArrangeLayersParents() => LayerCollection.ArrangeLayersParents(this.RootLayers);
        

        /// <summary>
        /// 
        /// </summary>
        public static void ArrangeLayersParents(IList<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }
        public static void _arrangeLayersParents(ILayer parents, IList<ILayer> layers)
        {
            foreach (ILayer layer in layers)
            {
                layer.Parents = parents;
                LayerCollection._arrangeLayersParents(layer, layer.Children);
            }
        }


        /// <summary>
        /// 
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