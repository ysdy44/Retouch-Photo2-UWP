using Retouch_Photo2.Layers;
using Windows.UI.Xaml;
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
                LayerCollection.ItemClick += (layer) =>
                {
                    if (layer.IsSelected == true)
                    {
                        this.ShowLayerMenu(layer);
                    }
                    else
                    {
                        this.ItemClick(layer);
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
                LayerCollection.VisibilityChanged += (layer) =>
                {
                    Visibility value = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                    layer.Visibility = value;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (LayerCollection.IsExpandChanged == null)
            {
                LayerCollection.IsExpandChanged += (layer) =>
                {
                    layer.IsExpand = !layer.IsExpand;
                    LayerCollection.ArrangeLayersVisibility(layer);

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            if (LayerCollection.IsSelectedChanged == null)
            {
                LayerCollection.IsSelectedChanged += (layer) =>
                {
                    layer.IsSelected = !layer.IsSelected;

                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                    LayerCollection.ArrangeLayersBackgroundItemClick(layer);

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
            
            if (LayerCollection.DragItemsStarted == null)
            {
                LayerCollection.DragItemsStarted += (layer, isSelected) =>
                {
                    this.DragSourceLayer = layer;
                    this.DragLayerIsSelected = isSelected;
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
                    LayerCollection.DragComplete(this.ViewModel.Layers, this.DragDestinationLayer, this.DragSourceLayer, this.DragLayerOverlayMode, this.DragLayerIsSelected);

                    LayerCollection.ArrangeLayersControls(this.ViewModel.Layers);
                    LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                    this.DragSourceLayer = null;
                    this.DragDestinationLayer = null;
                    this.DragLayerIsSelected =  false;
                    this.DragLayerOverlayMode = OverlayMode.None;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        private void ItemClick(ILayer selectedLayer)
        {
            //Is it independent of other layers?
            bool isfreedom = this.SettingViewModel.KeyCtrl;
            //Is select successively?
            bool isLinear = this.SettingViewModel.KeyShift;

            //Select a layer.
            if (isfreedom)
            {
                selectedLayer.IsSelected = !selectedLayer.IsSelected;
                LayerCollection.ArrangeLayersBackgroundItemClick(selectedLayer);
            }
            else if (isLinear) LayerCollection.ShiftSelectCurrentLayer(this.ViewModel.Layers, selectedLayer);
            else
            {
                //Selection
                this.SelectionViewModel.SetValue((layer)=>
                {
                    if (layer!= selectedLayer)
                    {
                        layer.IsSelected = false;
                    }
                });
                selectedLayer.IsSelected = true;

                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection

                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.Layers);

                this.ViewModel.Invalidate();//Invalidate
            }

        }

    }
}