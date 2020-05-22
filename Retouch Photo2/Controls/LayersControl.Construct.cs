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
            if (LayerCollection.ItemClick == null)
            {
                LayerCollection.ItemClick += (layerage) =>
                {
                    ILayer layer = layerage.Self;

                    if (layer.IsSelected == true)
                    {
                        this.ShowLayerMenu(layerage);
                    }
                    else
                    {
                        this.ItemClick(layerage);
                    }
                };
            }
            if (LayerCollection.RightTapped == null)
            {
                LayerCollection.RightTapped += (layer) =>
                {
                    this.ShowLayerMenu(layer);
                };
            }

            if (LayerCollection.VisibilityChanged == null)
            {
                LayerCollection.VisibilityChanged += (layerage) =>
                {
                    ILayer layer = layerage.Self;

                    //History 
                    LayersPropertyHistory history = new LayersPropertyHistory("Set visibility");
                    var previous = layer.Visibility;
                    history.UndoActions.Push(() =>
                    {
                        ILayer layer2 = layerage.Self;

                        layer2.Visibility = previous;
                    });

                    //History
                    this.ViewModel.HistoryPush(history);

                    Visibility value = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
                    layer.Visibility = value;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (LayerCollection.IsExpandChanged == null)
            {
                LayerCollection.IsExpandChanged += (layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.IsExpand = !layer.IsExpand;

                    LayerCollection.ArrangeLayersVisibility(layerage);
                };
            }
            if (LayerCollection.IsSelectedChanged == null)
            {                
                LayerCollection.IsSelectedChanged +=(isSelected)=> this.ViewModel.MethodSelectedNot(isSelected);//Method
            }

            if (LayerCollection.DragItemsStarted == null)
            {
                LayerCollection.DragItemsStarted += (layer, manipulationMode) =>
                {
                    this.DragSourceLayer = layer;

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
            if (LayerCollection.DragItemsDelta == null)
            {
                LayerCollection.DragItemsDelta += (layer, overlayMode) =>
                {
                    this.DragDestinationLayer = layer;
                    this.DragLayerOverlayMode = overlayMode;
                };
            }
            if (LayerCollection.DragItemsCompleted == null)
            {
                LayerCollection.DragItemsCompleted += () =>
                {
                    LayerCollection.DragComplete(this.ViewModel.LayerCollection, this.DragDestinationLayer, this.DragSourceLayer, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection
                    LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
                    LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                    this.ViewModel.Invalidate();//Invalidate

                    this.DragSourceLayer = null;
                    this.DragDestinationLayer = null;
                    this.DragLayerIsSelected = false;
                    this.DragLayerOverlayMode = OverlayMode.None;
                };
            }
        }


        private void ItemClick(Layerage selectedLayerage)
        {
            ILayer selectedLayer = selectedLayerage.Self;

            //Is it independent of other layers?
            bool isfreedom = this.SettingViewModel.KeyCtrl;
            //Is select successively?
            bool isLinear = this.SettingViewModel.KeyShift;


            if (isfreedom)                           
                this.ViewModel.MethodSelectedNot(selectedLayerage);//Method
            
            else if (isLinear)       
                LayerCollection.ShiftSelectCurrentLayer(this.ViewModel.LayerCollection, selectedLayerage);
            
            else this.ViewModel.MethodSelectedNew(selectedLayerage);  //Method
            

        }
    }
}