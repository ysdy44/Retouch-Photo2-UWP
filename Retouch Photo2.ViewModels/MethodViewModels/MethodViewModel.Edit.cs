using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {

        readonly Clipboard Clipboard = new Clipboard();

        public bool ClipboardEnable
        {
            get => this.clipboardEnable;
            set
            {
                this.clipboardEnable = value;
                this.OnPropertyChanged(nameof(ClipboardEnable));//Notify 
            }
        }
        private bool clipboardEnable;


        //Edit
        //private Layerage CloneLayerageCore(ILayer source)
        //{
        //   ILayer clone = source.Clone();
        //   Layerage layerageClone = clone.ToLayerage();
        //   LayerBase.Instances.Add(clone);
        //
        //    return layerageClone;
        // }



        public void MethodEditUndo()
        {
            bool isUndo = HistoryBase.Pull();

            if (isUndo)
            {
                this.SetMode();//Selection          
                LayerManager.ArrangeLayers();
                LayerManager.ArrangeLayersBackground();

                this.Invalidate();//Invalidate
            }

            this.IsUndoEnabled = HistoryBase.IsUndoEnabled;
        }

        public void MethodEditCut()
        {
            this.Clipboard.SetMode();//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_CutLayers);
            this.HistoryPush(history);

            LayerManager.RemoveAllSelected();//Remove

            this.SetMode();//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        public void MethodEditDuplicate()
        {
            this.Clipboard.SetMode();//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //Clipboard
            switch (this.Clipboard.SelectionMode)
            {
                case ListViewSelectionMode.None: return;
                case ListViewSelectionMode.Single:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_DuplicateLayer);
                        this.HistoryPush(history);

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerManager.PasteLayerage(layerage);
                        LayerManager.Mezzanine(layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_DuplicateLayers);
                        this.HistoryPush(history);

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerManager.PasteLayerages(layerages);
                        LayerManager.MezzanineRange(layerageClones);
                    }
                    break;
            }

            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.SetMode();
            this.Invalidate();//Invalidate                          
        }

        public void MethodEditCopy()
        {
            this.Clipboard.SetMode();//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton
        }

        public void MethodEditPaste()
        {
            //Clipboard
            switch (this.Clipboard.SelectionMode)
            {
                case ListViewSelectionMode.None: return;
                case ListViewSelectionMode.Single:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_PasteLayers);
                        this.HistoryPush(history);

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerManager.PasteLayerage(layerage);
                        LayerManager.Mezzanine(layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_PasteLayers);
                        this.HistoryPush(history);

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerManager.PasteLayerages(layerages);
                        LayerManager.MezzanineRange(layerageClones);
                    }
                    break;
            }

            this.SetMode();//Selection

            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();

            this.Invalidate();//Invalidate        
        }

        public void MethodEditClear()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_ClearLayers);
            this.HistoryPush(history);

            LayerManager.RemoveAllSelected();//Remove

            this.SetMode();//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }



        //Select
        private void SelectAllCore(bool isSelected)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            //Selection
            foreach (Layerage child in LayerManager.RootLayerage.Children)
            {
                ILayer layer = child.Self;

                //History
                var previous = layer.IsSelected;
                if (previous != isSelected)
                {
                    history.UndoAction += () =>
                    {
                        layer.IsSelected = previous;
                    };
                }

                layer.IsSelected = isSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode();//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        public void MethodSelectAll() => this.SelectAllCore(true);

        public void MethodSelectDeselect() => this.SelectAllCore(false);

        public void MethodSelectInvert()
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetIsSelected);

            //Selection
            foreach (Layerage child in LayerManager.RootLayerage.Children)
            {
                ILayer layer = child.Self;

                //History
                var previous = layer.IsSelected;
                history.UndoAction += () =>
                {
                    layer.IsSelected = previous;
                };

                layer.IsSelected = !layer.IsSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode();//Selection
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }



        //Group
        public void MethodGroupGroup()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_GroupLayers);
            this.HistoryPush(history);

            LayerManager.GroupAllSelectedLayers();

            this.SetMode();
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        public void MethodGroupUngroup()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_UngroupLayers);
            this.HistoryPush(history);

            LayerManager.UngroupAllSelectedLayer();

            this.SetMode();
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }

        public void MethodGroupRelease()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_ReleaseLayers);
            this.HistoryPush(history);

            //Selection
            this.SetValue((layerage) =>
            {
                LayerManager.ReleaseGroupLayer(layerage);
            });

            this.SetMode();
            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.Invalidate();//Invalidate
        }



        //ConvertToCurves
        public void MethodConvertToCurves()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_AddLayer_ConvertToCurves);
            this.HistoryPush(history);

            this.SetValue((layerage) =>
            {
                ILayer layer = layerage.Self;

                //Turn to curve layer
                ILayer curveLayer = this.MethodConvertToCurves_CreateCurveLayer(layer);

                if (curveLayer != null)
                {
                    Layerage curveLayerage = curveLayer.ToLayerage();
                    string id = curveLayerage.Id;
                    LayerBase.Instances.Add(id, curveLayer);

                    //set image brush
                    if (layer.Type == LayerType.Image)
                    {
                        ImageLayer imageLayer = (ImageLayer)layer;
                        curveLayer.Style.Fill = imageLayer.ToBrush();
                    }

                    this.MethodConvertToCurves_ReplaceLayerage(curveLayerage, layerage);
                }
            });

            LayerManager.ArrangeLayers();
            LayerManager.ArrangeLayersBackground();
            this.SetMode();//Selection
        }

        //Create curve layer
        private ILayer MethodConvertToCurves_CreateCurveLayer(ILayer layer)
        {
            NodeCollection nodes = layer.ConvertToCurves(LayerManager.CanvasDevice);
            if (nodes == null) return null;

            if (nodes.Count > 3)
            {
                CurveLayer curveLayer = new CurveLayer(nodes)
                {
                    IsSelected = true,
                };
                LayerBase.CopyWith(layer, curveLayer);
                return curveLayer;
            }

            return null;
        }
        //Replace curveLayerage to layerage
        private void MethodConvertToCurves_ReplaceLayerage(Layerage curveLayerage, Layerage layerage)
        {
            Layerage parents = LayerManager.GetParentsChildren(layerage);
            int index = parents.Children.IndexOf(layerage);
            parents.Children[index] = curveLayerage;
        }


    }
}