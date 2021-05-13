using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Input;
using Retouch_Photo2.Menus;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //LayerManager
        Layerage DragSourceLayerage;
        Layerage DragDestinationLayerage;
        bool DragLayerIsSelected;
        OverlayMode DragLayerOverlayMode;


        //LayerManager
        private void ConstructLayerManager()
        {
            this.LayersScrollViewer.Tapped += (s, e) => this.MethodViewModel.MethodSelectedNone();//Method
            this.LayersScrollViewer.Holding += (s, e) => this.LayersRightTapped();
            this.LayersScrollViewer.RightTapped += (s, e) => this.LayersRightTapped();
        }


        private void LayerRightTapped(ILayer layer)
        {
            Expander.ShowAt(MenuType.Layer, layer.Control);
        }
        private void LayersRightTapped()
        {
            Expander.ShowAt(MenuType.Layer, this.LayersScrollViewer);
        }
        private void LayerVisibilityChanged(ILayer layer2)
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
        }
        private void LayerIsExpandChanged(ILayer layer)
        {
            layer.IsExpand = !layer.IsExpand;

            Layerage layerage = LayerManager.FindFirstLayerage(layer);
            LayerManager.ArrangeLayersIsExpand(layerage);
        }
        private void LayerIsSelectedChanged(ILayer layer)
        {
            Layerage layerage = LayerManager.FindFirstLayerage(layer);
            this.MethodViewModel.MethodSelectedNot(layerage);//Method
        }
        private void LayerDragItemsStarted(ILayer layer, ManipulationModes manipulationModes)
        {
            Layerage layerage = LayerManager.FindFirstLayerage(layer);

            this.DragSourceLayerage = layerage;

            if (manipulationModes == ManipulationModes.TranslateY)
            {
                this.DragLayerIsSelected = true;
            }
            else if (manipulationModes == ManipulationModes.System)
            {
                this.DragLayerIsSelected = false;
            }
        }
        private void LayerDragItemsDelta(ILayer layer, OverlayMode overlayMode)
        {
            Layerage layerage = LayerManager.FindFirstLayerage(layer);

            this.DragDestinationLayerage = layerage;
            this.DragLayerOverlayMode = overlayMode;
        }
        private void LayerDragItemsCompleted()
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
        }



        private void LayerItemClick(ILayer layer)
        {
            Layerage selectedLayerage = LayerManager.FindFirstLayerage(layer);

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