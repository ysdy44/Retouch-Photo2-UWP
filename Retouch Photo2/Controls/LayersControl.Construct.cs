using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Tools;
using Windows.UI.Xaml;
using FanKit.Transformers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public partial class LayersControl : UserControl
    {

        private void ConstructLayerCollection()
        {
            if (LayerageCollection.ItemClick == null)
            {
                LayerageCollection.ItemClick += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);

                    //if (layer.IsSelected == true)
                    //{
                    // this.ShowLayerMenu(layerage);
                    //}
                    //else
                    //{
                    this.ItemClick(layerage);
                    //}
                };
            }
            if (LayerageCollection.RightTapped == null)
            {
                LayerageCollection.RightTapped += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);

                    this.ShowLayerMenu(layerage);
                };
            }

            if (LayerageCollection.VisibilityChanged == null)
            {
                LayerageCollection.VisibilityChanged += (layer) =>
                { 
                    //History 
                    LayersPropertyHistory history = new LayersPropertyHistory("Set visibility");
                    var previous = layer.Visibility;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layer;

                        layer2.Visibility = previous;
                    });

                    //History
                    this.ViewModel.HistoryPush(history);

                    Visibility value = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                    layer.Visibility = value;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (LayerageCollection.IsExpandChanged == null)
            {
                LayerageCollection.IsExpandChanged += (layer) =>
                {
                    layer.IsExpand = !layer.IsExpand;

                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);
                    LayerageCollection.ArrangeLayersVisibility(layerage);
                };
            }
            if (LayerageCollection.IsSelectedChanged == null)
            {
                LayerageCollection.IsSelectedChanged += (layer) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);
                    this.MethodViewModel.MethodSelectedNot(layerage);//Method
                 };
            }

            if (LayerageCollection.DragItemsStarted == null)
            {
                LayerageCollection.DragItemsStarted += (layer, manipulationMode) =>
                {
                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);

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
                    Layerage layerage = this.ViewModel.LayerageCollection.FindLayerage_ByILayer(layer);

                    this.DragDestinationLayerage = layerage;
                    this.DragLayerOverlayMode = overlayMode;
                };
            }
            if (LayerageCollection.DragItemsCompleted == null)
            {
                LayerageCollection.DragItemsCompleted += () =>
                {
                    LayerageCollection.DragComplete(this.ViewModel.LayerageCollection, this.DragDestinationLayerage, this.DragSourceLayerage, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                    LayerageCollection.ArrangeLayersControls(this.ViewModel.LayerageCollection);
                    LayerageCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerageCollection);
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
                this.MethodViewModel.MethodSelectedNew(selectedLayerage);  //Method
        }

    }
}