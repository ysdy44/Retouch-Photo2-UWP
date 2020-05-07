using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        /// <summary>
        /// Remove a layer.
        /// </summary>      
        public void RemoveLayer(ILayer removeLayer)
        {
            IList<ILayer> layers = (removeLayer.Parents == null) ? this.RootLayers : removeLayer.Parents.Children;

            this._removeLayer(removeLayer, layers);
        }

        /// <summary>
        /// Remove all selected layers.
        /// </summary>
        public void RemoveAllSelectedLayers() => this._removeAllSelectedLayers(this.RootLayers);


        private void _removeAllSelectedLayers(IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                //Recursive
                if (child.SelectMode.ToBool())
                    this._removeAllLayers(child.Children);
                //Recursive
                else
                    this._removeAllSelectedLayers(child.Children);
            }

            //Remove
            ILayer removeLayer = null;
            do
            {
                this._removeLayer(removeLayer, layers);

                removeLayer = layers.FirstOrDefault(layer => layer.SelectMode == SelectMode.Selected);
            }
            while (removeLayer != null);
        }

        private void _removeAllLayers(IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                //Recursive
                this._removeAllLayers(child.Children);

                this.RootControls.Remove(child.Control.Self);
            }
            layers.Clear();
        }

        private void _removeLayer(ILayer removeLayer, IList<ILayer> layers)
        {
            if (removeLayer != null)
            {
                this.RootControls.Remove(removeLayer.Control.Self);
                layers.Remove(removeLayer);

                if (removeLayer.Parents != null)
                {
                    if (removeLayer.Parents.Children.Count == 0)
                    {
                        removeLayer.Parents.ExpandMode = ExpandMode.NoChildren;
                    }
                }
            }
        }
        

    }
}