using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "ViewModel" />. 
    /// </summary>
    public partial class ViewModel : INotifyPropertyChanged
    {

        Clipboard Clipboard = new Clipboard();

        public bool ClipboardEnable
        {
            get => this.clipboardEnable;
            set
            {
                this.clipboardEnable = value;
                this.OnPropertyChanged(nameof(this.ClipboardEnable));//Notify 
            }
        }
        private bool clipboardEnable;


        //Edit
        private Layerage _cloneLayerage(ILayer source)
        {
            ILayer clone = source.Clone(this.CanvasDevice);
            Layerage layerageClone = clone.ToLayerage();
            Layer.Instances.Add(clone);

            return layerageClone;
        }



        public void MethodEditUndo()
        {
            bool isUndo = this.HistoryUndo();//History

            if (isUndo)
            {
                this.SetMode(this.LayerageCollection);//Selection          
                LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
                LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);

                this.Invalidate();//Invalidate
            }
        }

        public void MethodEditCut()
        {
            this.Clipboard.SetMode(this.LayerageCollection);//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //History
            this.HistoryPushLayeragesHistory("Cut layers");

            LayerageCollection.RemoveAllSelectedLayers(this.LayerageCollection);//Remove

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodEditDuplicate()
        {
            this.Clipboard.SetMode(this.LayerageCollection);//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //Clipboard
            switch (this.Clipboard.SelectionMode)
            {
                case ListViewSelectionMode.None: return;
                case ListViewSelectionMode.Single:
                    {
                        //History
                        this.HistoryPushLayeragesHistory("Duplicate layers");

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerageCollection.CopyLayerage(this.CanvasDevice, layerage);
                        LayerageCollection.Mezzanine(this.LayerageCollection, layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        this.HistoryPushLayeragesHistory("Duplicate layers");

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerageCollection.CopyLayerages(this.CanvasDevice, layerages);
                        LayerageCollection.MezzanineRange(this.LayerageCollection, layerageClones);
                    }
                    break;
            }

            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.SetMode(this.LayerageCollection);
            this.Invalidate();//Invalidate                          
        }

        public void MethodEditCopy()
        {
            this.Clipboard.SetMode(this.LayerageCollection);//Clipboard
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
                        this.HistoryPushLayeragesHistory("Paste layers");

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerageCollection.CopyLayerage(this.CanvasDevice, layerage);
                        LayerageCollection.Mezzanine(this.LayerageCollection, layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        this.HistoryPushLayeragesHistory("Paste layers");

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerageCollection.CopyLayerages(this.CanvasDevice, layerages);
                        LayerageCollection.MezzanineRange(this.LayerageCollection, layerageClones);
                    }
                    break;
            }

            this.SetMode(this.LayerageCollection);//Selection

            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);

            this.Invalidate();//Invalidate        
        }

        public void MethodEditClear()
        {
            //History
            this.HistoryPushLayeragesHistory("Clear layers");

            LayerageCollection.RemoveAllSelectedLayers(this.LayerageCollection);//Remove

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        

        //Select
        private void _setAllIsSelected(bool isSelected)
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set is selected");

            //Selection
            foreach (Layerage layerage in this.LayerageCollection.RootLayerages)
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.IsSelected;
                if (previous != isSelected)
                {
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.IsSelected = previous;
                    });
                }

                layer.IsSelected = isSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }
        
        public void MethodSelectAll() => this._setAllIsSelected(true);

        public void MethodSelectDeselect() => this._setAllIsSelected(false);

        public void MethodSelectInvert()
        {
            //History
            LayersPropertyHistory history = new LayersPropertyHistory("Set is selected");

            //Selection
            foreach (Layerage layerage in this.LayerageCollection.RootLayerages)
            {
                ILayer layer = layerage.Self;

                //History
                var previous = layer.IsSelected;
                history.UndoActions.Push(() =>
                {
                    ILayer layer2 = layerage.Self;

                    layer2.IsSelected = previous;
                });

                layer.IsSelected = !layer.IsSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }
        


        //Group
        public void MethodGroupGroup()
        {
            //History
            this.HistoryPushLayeragesHistory("Group layers");

            LayerageCollection.GroupAllSelectedLayers(this.LayerageCollection);

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodGroupUnGroup()
        {
            //History
            this.HistoryPushLayeragesHistory("UnGroup layers");

            LayerageCollection.UnGroupAllSelectedLayer(this.LayerageCollection);

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodGroupRelease()
        {
            //History
            this.HistoryPushLayeragesHistory("Release layers");

            //Selection
            this.SetValue((layerage) =>
            {
                LayerageCollection.ReleaseGroupLayer(this.LayerageCollection, layerage);
            });

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayersControls(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }



    }
}