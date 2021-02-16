using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //LayerManager
        private void ConstructLayerManager()
        {
            if (LayerManager.ItemClick == null)
            {
                LayerManager.ItemClick += (layer) =>
                {
                    Layerage layerage = LayerManager.FindFirstLayerage(layer);

                    this.ItemClick(layerage);
                };
            }
            if (LayerManager.RightTapped == null)
            {
                LayerManager.RightTapped += (layer) =>
                {
                    Layerage layerage = LayerManager.FindFirstLayerage(layer);

                    this.ShowLayerMenu(layerage);
                };
            }

            if (LayerManager.VisibilityChanged == null)
            {
                LayerManager.VisibilityChanged += (layer2) =>
                {
                    Visibility visibility = (layer2.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                    if (layer2.IsSelected)
                    {
                        this.SelectionViewModel.Visibility = visibility;

                        this.MethodViewModel.ILayerChanged<Visibility>
                        (
                            set: (layer) => layer.Visibility = visibility,

                            type: HistoryType.LayersProperty_SetVisibility,
                            getUndo: (layer) => layer.Visibility,
                            setUndo: (layer, previous) => layer.Visibility = previous
                        );
                    }
                    else
                    {
                        //History
                        LayersPropertyHistory history = new LayersPropertyHistory(HistoryType.LayersProperty_SetVisibility);

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
            if (LayerManager.IsExpandChanged == null)
            {
                LayerManager.IsExpandChanged += (layer) =>
                {
                    layer.IsExpand = !layer.IsExpand;

                    Layerage layerage = LayerManager.FindFirstLayerage(layer);
                    LayerManager.ArrangeLayersIsExpand(layerage);
                };
            }
            if (LayerManager.IsSelectedChanged == null)
            {
                LayerManager.IsSelectedChanged += (layer) =>
                {
                    Layerage layerage = LayerManager.FindFirstLayerage(layer);
                    this.MethodViewModel.MethodSelectedNot(layerage);//Method
                };
            }

            if (LayerManager.DragItemsStarted == null)
            {
                LayerManager.DragItemsStarted += (layer, manipulationMode) =>
                {
                    Layerage layerage = LayerManager.FindFirstLayerage(layer);

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
            if (LayerManager.DragItemsDelta == null)
            {
                LayerManager.DragItemsDelta += (layer, overlayMode) =>
                {
                    Layerage layerage = LayerManager.FindFirstLayerage(layer);

                    this.DragDestinationLayerage = layerage;
                    this.DragLayerOverlayMode = overlayMode;
                };
            }
            if (LayerManager.DragItemsCompleted == null)
            {
                LayerManager.DragItemsCompleted += () =>
                {
                    //History
                    LayeragesArrangeHistory history = new LayeragesArrangeHistory(HistoryType.LayeragesArrange_LayersArrange);
                    this.ViewModel.HistoryPush(history);

                    LayerManager.DragComplete(this.DragDestinationLayerage, this.DragSourceLayerage, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    this.SelectionViewModel.SetMode();//Selection
                    LayerManager.ArrangeLayers();
                    LayerManager.ArrangeLayersBackground();
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