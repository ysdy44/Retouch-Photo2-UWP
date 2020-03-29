using FanKit.Transformers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Linq;

namespace Retouch_Photo2.Layers
{
    public partial class LayerCollection
    {

        #region Group


        /// <summary>
        /// Group all selected layers.
        /// </summary>
        public void GroupAllSelectedLayers()
        {
            //Layers
            ILayer outermostLayerParents = this._findOutermostSelectedLayerParents();
            IList<ILayer> outermostLayers = (outermostLayerParents == null) ? this.RootLayers : outermostLayerParents.Children;

            //Insert
            ILayer insertIayer = outermostLayers.FirstOrDefault(e => e.SelectMode == SelectMode.Selected);
            if (insertIayer == null) return;
            int insertIndex = outermostLayers.IndexOf(insertIayer);

            //GroupLayer
            ILayer groupLayer = this._createGroupLayer(insertIayer);
            groupLayer.Parents = outermostLayerParents;

            //Temp
            {
                IList<ILayer> tempGrouplayers = this._createTempGrouplayers();
                if (tempGrouplayers.Count == 0) return;
                this._addLayersToGroupLayer(groupLayer, tempGrouplayers);

                Transformer transformer = LayerCollection.RefactoringTransformer(tempGrouplayers);
                groupLayer.TransformManager = new TransformManager(transformer);
            }

            //Arrange
            {
                this._noneAllLayers(this.RootLayers);

                outermostLayers.Insert(insertIndex, groupLayer);
                groupLayer.SelectMode = SelectMode.Selected;
                groupLayer.ExpandMode = ExpandMode.UnExpand;
            }
        }

        private ILayer _createGroupLayer(ILayer insertIayer)
        {
            ILayer insertParents = insertIayer.Parents;

            ILayer groupLayer = new GroupLayer
            {
                Parents = insertParents,
            };  
            groupLayer.Control.Text = "Group";
            return groupLayer;
        }

        private IList<ILayer> _createTempGrouplayers()
        {
            IList<ILayer> tempGrouplayers = new List<ILayer>();

            void _addTempGrouplayers(IList<ILayer> layers)
            {
                foreach (ILayer child in layers)
                {
                    if (child.SelectMode == SelectMode.Selected)
                    {
                        tempGrouplayers.Add(child);
                    }
                    else
                    {
                        //Recursive
                        _addTempGrouplayers(child.Children);
                    }
                }
            }

            //Recursive
            _addTempGrouplayers(this.RootLayers);

            return tempGrouplayers;
        }

        private void _addLayersToGroupLayer(ILayer groupLayer, IList<ILayer> tempGrouplayers)
        {
            foreach (ILayer child in tempGrouplayers)
            {
                child.SelectMode = SelectMode.None;
                child.SelectMode = SelectMode.ParentsSelected;

                LayerCollection.Disengage(child, this);
                child.Parents = groupLayer;

                groupLayer.Children.Add(child);
            }
        }

        private ILayer _findOutermostSelectedLayerParents()
        {
            ILayer outermostGrouplayers = null;

            void _findOutermostSelectedLayer(ILayer layer)
            {
                if (outermostGrouplayers == null)
                {
                    if (layer.SelectMode == SelectMode.Selected)
                    {
                        outermostGrouplayers = layer.Parents;
                    }
                    else
                    {
                        foreach (ILayer child in layer.Children)
                        {
                            //Recursive
                            _findOutermostSelectedLayer(child);
                        }
                    }
                }
            }

            foreach (ILayer child in this.RootLayers)
            {
                //Recursive
                _findOutermostSelectedLayer(child);
            }

            return outermostGrouplayers;
        }

        private void _noneAllLayers(IList<ILayer> layers)
        {
            foreach (ILayer child in layers)
            {
                child.SelectMode = SelectMode.None;
                this._noneAllLayers(child.Children);
            }
        }


        #endregion

    }
}