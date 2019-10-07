using System.Collections.Generic;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {
        
        /// <summary>
        /// Gets all selected layers.
        /// </summary>
        /// <returns> The selected layers. </returns>
        public IList<ILayer> GetAllSelectedLayers()
        {
            IList<ILayer> addLayers = new List<ILayer>();

            void addLayer(IList<ILayer> layers)
            {
                foreach (ILayer child in layers)
                {
                    if (child.SelectMode == SelectMode.Selected)
                    {
                        addLayers.Add(child);
                    }

                    //Recursive
                    addLayer(child.Children);
                }
            }

            //Recursive
            addLayer(this.RootLayers);

            return addLayers;
        }
               

        #region ShiftSelect


        public void ShiftSelectCurrentLayer(ILayer currentLayer)
        {
            IList<ILayer> parentsChildren = (currentLayer.Parents == null) ?
                this.RootLayers : currentLayer.Parents.Children;

            //Recursive
            bool isFind = this._findShiftSelectedLayer(currentLayer, parentsChildren);
            if (isFind) this._setShiftSelectedLayer(currentLayer, parentsChildren);
        }
        
        private bool _findShiftSelectedLayer(ILayer currentLayer, IList<ILayer> layers)
        {
            // Find self layer and selected layer.
            bool existSelectedLayer = false;
            bool existSelfLayer = false;

            foreach (ILayer child in layers)
            {
                if (existSelectedLayer == false)
                    if (child.SelectMode == SelectMode.Selected || child.SelectMode == SelectMode.ChildSelected)
                        existSelectedLayer = true;

                if (existSelfLayer == false)
                    if (child == currentLayer)
                        existSelfLayer = true;
            }

            return (existSelectedLayer && existSelfLayer);
        }

        private void _setShiftSelectedLayer(ILayer currentLayer, IList<ILayer> layers)
        {
            // Find self layer and selected layer.
            bool existSelectedLayer = false;
            bool existSelfLayer = false;

            foreach (ILayer child in layers)
            {
                if (existSelectedLayer && existSelfLayer)
                    break;

                if (existSelectedLayer || existSelfLayer)
                    child.SelectMode = SelectMode.Selected;


                if (existSelectedLayer == false)
                    if (child.SelectMode == SelectMode.Selected || child.SelectMode == SelectMode.ChildSelected)
                        existSelectedLayer = true;

                if (existSelfLayer == false)
                    if (child == currentLayer)
                        existSelfLayer = true;
            }

            currentLayer.SelectMode = SelectMode.Selected;
        }
               

        #endregion
        
    }
}