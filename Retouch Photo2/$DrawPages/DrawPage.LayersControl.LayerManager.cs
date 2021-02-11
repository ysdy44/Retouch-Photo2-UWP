using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2
{
    /// <summary> 
    /// Represents a page used to draw vector graphics.
    /// </summary>
    public sealed partial class DrawPage : Page
    {

        //LayerManager
        private void ConstructLayerManager()
        {
            if (LayerageCollection.ItemClick == null)
            {
                LayerageCollection.ItemClick += (layer) =>
                {
                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);

                    this.ItemClick(layerage);
                };
            }
            if (LayerageCollection.RightTapped == null)
            {
                LayerageCollection.RightTapped += (layer) =>
                {
                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);

                    this.ShowLayerMenu(layerage);
                };
            }

            if (LayerageCollection.VisibilityChanged == null)
            {
                LayerageCollection.VisibilityChanged += (layer2) =>
                {
                    Visibility visibility = (layer2.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                    if (layer2.IsSelected)
                    {
                        this.SelectionViewModel.Visibility = visibility;

                        this.MethodViewModel.ILayerChanged<Visibility>
                        (
                            set: (layer) => layer.Visibility = visibility,

                            historyTitle: "Set visibility",
                            getHistory: (layer) => layer.Visibility,
                            setHistory: (layer, previous) => layer.Visibility = previous
                        );
                    }
                    else
                    {
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory("Set visibility");

                        //Selection
                        ILayer layer = layer2;

                        var previous = layer.Visibility;
                        history.UndoAction += () =>
                        {
                            //Refactoring
                            layer.IsRefactoringRender = true;
                            layer.IsRefactoringIconRender = true;
                            layer.Visibility = previous;
                        };

                        //Refactoring
                        layer.IsRefactoringRender = true;
                        layer.IsRefactoringIconRender = true;
                        //layerage.RefactoringParentsRender();
                        //layerage.RefactoringParentsIconRender();
                        layer.Visibility = visibility;

                        //History
                        this.ViewModel.HistoryPush(history);

                        this.ViewModel.Invalidate();//Invalidate
                    }
                };
            }
            if (LayerageCollection.IsExpandChanged == null)
            {
                LayerageCollection.IsExpandChanged += (layer) =>
                {
                    layer.IsExpand = !layer.IsExpand;

                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);
                    LayerageCollection.ArrangeLayersIsExpand(layerage);
                };
            }
            if (LayerageCollection.IsSelectedChanged == null)
            {
                LayerageCollection.IsSelectedChanged += (layer) =>
                {
                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);
                    this.MethodViewModel.MethodSelectedNot(layerage);//Method
                };
            }

            if (LayerageCollection.DragItemsStarted == null)
            {
                LayerageCollection.DragItemsStarted += (layer, manipulationMode) =>
                {
                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);

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
                    Layerage layerage = LayerageCollection.FindFirstLayerage(layer);

                    this.DragDestinationLayerage = layerage;
                    this.DragLayerOverlayMode = overlayMode;
                };
            }
            if (LayerageCollection.DragItemsCompleted == null)
            {
                LayerageCollection.DragItemsCompleted += () =>
                {
                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory("Layers arrange");
                    this.ViewModel.HistoryPush(history);

                    LayerageCollection.DragComplete(this.DragDestinationLayerage, this.DragSourceLayerage, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    this.SelectionViewModel.SetMode();//Selection
                    LayerageCollection.ArrangeLayers();
                    LayerageCollection.ArrangeLayersBackground();
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
            //ILayer selectedLayer = selectedLayerage.Self;

            //Is it independent of other layers?
            bool isfreedom = this.SettingViewModel.KeyCtrl | this.SettingViewModel.KeyShift;
            //bool isfreedom = this.SettingViewModel.KeyCtrl;
            //Is select successively?
            //bool isLinear = this.SettingViewModel.KeyShift;

            if (isfreedom)
                this.MethodViewModel.MethodSelectedNot(selectedLayerage);//Method

            // else if (isLinear)       
            //LayerManager.ShiftSelectCurrentLayer(selectedLayerage);

            else
                this.MethodViewModel.MethodSelectedNew(selectedLayerage);//Method
        }


    }
}