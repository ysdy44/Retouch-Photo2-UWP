using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        

        #region Remove


        /// <summary>
        /// Remove
        /// </summary>
        public void RemoveAllSelectedLayers() => this._removeAllSelectedLayers(this.RootLayers);

        private void _removeAllSelectedLayers(IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                if (child.SelectMode.ToBool())
                {
                    //Recursive
                    this._removeAllLayers(child.Children);
                }
                else
                {
                    //Recursive
                    this._removeAllSelectedLayers(child.Children);
                }
            }

            //Remove
            ILayer removeLayer = null;
            do
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


        #endregion

    }
}