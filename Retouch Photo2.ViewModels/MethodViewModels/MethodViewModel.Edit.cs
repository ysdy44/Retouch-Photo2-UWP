using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.ViewModels
{
    /// <summary> 
    /// Represents a ViewModel that contains some methods of the application
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
                this.OnPropertyChanged(nameof(ClipboardEnable));//Notify 
            }
        }
        private bool clipboardEnable;


        //Edit
        private Layerage _cloneLayerage(ILayer source)
        {
            ILayer clone = source.Clone(this.CanvasDevice);
            Layerage layerageClone = clone.ToLayerage();
            LayerBase.Instances.Add(clone);

            return layerageClone;
        }



        public void MethodEditUndo()
        {
            bool isUndo = this.HistoryUndo();//History

            if (isUndo)
            {
                this.SetMode(this.LayerageCollection);//Selection          
                LayerageCollection.ArrangeLayers(this.LayerageCollection);
                LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);

                this.Invalidate();//Invalidate
            }
        }

        public void MethodEditCut()
        {
            this.Clipboard.SetMode(this.CanvasDevice, this.LayerageCollection);//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Cut layers", this.LayerageCollection);
            this.HistoryPush(history);

            LayerageCollection.RemoveAllSelected(this.LayerageCollection);//Remove

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodEditDuplicate()
        {
            this.Clipboard.SetMode(this.CanvasDevice, this.LayerageCollection);//Clipboard
            this.ClipboardEnable = this.Clipboard.CanPaste;//PasteButton

            //Clipboard
            switch (this.Clipboard.SelectionMode)
            {
                case ListViewSelectionMode.None: return;
                case ListViewSelectionMode.Single:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory("Duplicate layer", this.LayerageCollection);
                        this.HistoryPush(history);

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerageCollection.PasteLayerage(this.CanvasDevice, layerage);
                        LayerageCollection.Mezzanine(this.LayerageCollection, layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory("Duplicate layers", this.LayerageCollection);
                        this.HistoryPush(history);

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerageCollection.PasteLayerages(this.CanvasDevice, layerages);
                        LayerageCollection.MezzanineRange(this.LayerageCollection, layerageClones);
                    }
                    break;
            }

            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.SetMode(this.LayerageCollection);
            this.Invalidate();//Invalidate                          
        }

        public void MethodEditCopy()
        {
            this.Clipboard.SetMode(this.CanvasDevice, this.LayerageCollection);//Clipboard
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
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory("Paste layers", this.LayerageCollection);
                        this.HistoryPush(history);

                        Layerage layerage = this.Clipboard.Layerage;
                        Layerage layerageClone = LayerageCollection.PasteLayerage(this.CanvasDevice, layerage);
                        LayerageCollection.Mezzanine(this.LayerageCollection, layerageClone);
                    }
                    break;
                case ListViewSelectionMode.Multiple:
                    {
                        //History
                        LayeragesArrangeHistory history = new LayeragesArrangeHistory("Paste layers", this.LayerageCollection);
                        this.HistoryPush(history);

                        IEnumerable<Layerage> layerages = this.Clipboard.Layerages;
                        IEnumerable<Layerage> layerageClones = LayerageCollection.PasteLayerages(this.CanvasDevice, layerages);
                        LayerageCollection.MezzanineRange(this.LayerageCollection, layerageClones);
                    }
                    break;
            }

            this.SetMode(this.LayerageCollection);//Selection

            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);

            this.Invalidate();//Invalidate        
        }

        public void MethodEditClear()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Clear layers", this.LayerageCollection);
            this.HistoryPush(history);

            LayerageCollection.RemoveAllSelected(this.LayerageCollection);//Remove

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
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
                    history.UndoAction += () =>
                    {
                        layer.IsSelected = previous;
                    };
                }

                layer.IsSelected = isSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
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
                history.UndoAction += () =>
                {
                    layer.IsSelected = previous;
                };

                layer.IsSelected = !layer.IsSelected;
            }

            //History
            this.HistoryPush(history);

            this.SetMode(this.LayerageCollection);//Selection
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }
        


        //Group
        public void MethodGroupGroup()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Group layers", this.LayerageCollection);
            this.HistoryPush(history);

            LayerageCollection.GroupAllSelectedLayers(this.CanvasDevice, this.LayerageCollection);

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodGroupUnGroup()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("UnGroup layers", this.LayerageCollection);
            this.HistoryPush(history);

            LayerageCollection.UnGroupAllSelectedLayer(this.LayerageCollection);

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }

        public void MethodGroupRelease()
        {
            //History
            LayeragesArrangeHistory history = new LayeragesArrangeHistory("Release layers", this.LayerageCollection);
            this.HistoryPush(history);

            //Selection
            this.SetValue((layerage) =>
            {
                LayerageCollection.ReleaseGroupLayer(this.LayerageCollection, layerage);
            });

            this.SetMode(this.LayerageCollection);
            LayerageCollection.ArrangeLayers(this.LayerageCollection);
            LayerageCollection.ArrangeLayersBackground(this.LayerageCollection);
            this.Invalidate();//Invalidate
        }



    }
}