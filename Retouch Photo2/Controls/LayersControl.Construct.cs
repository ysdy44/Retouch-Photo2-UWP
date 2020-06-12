using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Represents a control that arranges <see cref="LayerControl"/> vertically.
    /// </summary>
    public partial class LayersControl : UserControl
    {
        
        private void ConstructLayerageCollection()
        {
            if (LayerageCollection.ItemClick == null)
            {
                LayerageCollection.ItemClick += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);
                    
                    this.ItemClick(layerage);
                };
            }
            if (LayerageCollection.RightTapped == null)
            {
                LayerageCollection.RightTapped += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);

                    this.ShowLayerMenu(layerage);
                };
            }

            if (LayerageCollection.VisibilityChanged == null)
            {
                LayerageCollection.VisibilityChanged += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);
                    Visibility value = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                    //History 
                    LayersPropertyHistory history = new LayersPropertyHistory("Set visibility");

                    var previous = layer.Visibility;
                    history.UndoAction += () =>
                    {
                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.Visibility = previous;
                    };

                    //Refactoring
                    layer.IsRefactoringRender = true;
                    layerage.RefactoringParentsRender();
                    layerage.RefactoringParentsIconRender();
                    layer.Visibility = value;

                    //History
                    this.ViewModel.HistoryPush(history);

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (LayerageCollection.IsExpandChanged == null)
            {
                LayerageCollection.IsExpandChanged += (layer) =>
                {
                    layer.IsExpand = !layer.IsExpand;

                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);
                    LayerageCollection.ArrangeLayersVisibility(layerage);
                };
            }
            if (LayerageCollection.IsSelectedChanged == null)
            {
                LayerageCollection.IsSelectedChanged += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);
                    this.MethodViewModel.MethodSelectedNot(layerage);//Method
                 };
            }

            if (LayerageCollection.DragItemsStarted == null)
            {
                LayerageCollection.DragItemsStarted += (layer, manipulationMode) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);

                    this.DragSourceLayerage = layerage;

                    if (manipulationMode == ManipulationModes.TranslateY)
                    {
                        this.DragLayerIsSelected = true;
                    }
                    else if (manipulationMode == ManipulationModes.System)
                    {
                        this.DragLayerIsSelected = false;
                    }
                };
            }
            if (LayerageCollection.DragItemsDelta == null)
            {
                LayerageCollection.DragItemsDelta += (layer, overlayMode) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindFirstLayerage(layer);

                    this.DragDestinationLayerage = layerage;
                    this.DragLayerOverlayMode = overlayMode;
                };
            }
            if (LayerageCollection.DragItemsCompleted == null)
            {
                LayerageCollection.DragItemsCompleted += () =>
                {
                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange", this.ViewModel.LayerageCollection);
                    this.ViewModel.HistoryPush(history);

                    LayerageCollection.DragComplete(this.ViewModel.LayerageCollection, this.DragDestinationLayerage, this.DragSourceLayerage, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                    LayerageCollection.ArrangeLayersBackground(this.ViewModel.LayerageCollection);
                    this.ViewModel.Invalidate();//Invalidate

                    this.DragSourceLayerage = null;
                    this.DragDestinationLayerage = null;
                    this.DragLayerIsSelected = false;
                    this.DragLayerOverlayMode = OverlayMode.None;
                };
            }
        }


        private void ItemClick(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //Is it independent of other layers?
            bool isfreedom = this.SettingViewModel.KeyCtrl | this.SettingViewModel.KeyShift;
            //bool isfreedom = this.SettingViewModel.KeyCtrl;
            //Is select successively?
            //bool isLinear = this.SettingViewModel.KeyShift;

            if (isfreedom)                           
                this.MethodViewModel.MethodSelectedNot(selectedLayerage);//Method
            
           // else if (isLinear)       
                //LayerageCollection.ShiftSelectCurrentLayer(this.ViewModel.LayerageCollection, selectedLayerage);

            else
                this.MethodViewModel.MethodSelectedNew(selectedLayerage);//Method
        }

    }
}