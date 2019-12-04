using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
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
                    if (layer.SelectMode == SelectMode.Selected)
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
            if (LayerCollection.VisualChanged == null)
            {
                LayerCollection.VisualChanged += (layer) =>
                {
                    Visibility value = (layer.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

                    layer.Visibility = value;

                    this.ViewModel.Invalidate();
                };
            }
            if (LayerCollection.SelectChanged == null)
            {
                LayerCollection.SelectChanged += () =>
                {
                    this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                    this.ViewModel.Invalidate();
                };
            }

            if (LayerCollection.DragItemsStarted == null)
            {
                LayerCollection.DragItemsStarted += (layer, selectMode) =>
                {
                    this.DragSourceLayer = layer;
                    this.DragLayerSelectMode = selectMode;
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
                    this.ViewModel.Layers.DragComplete(this.DragDestinationLayer, this.DragSourceLayer, this.DragLayerOverlayMode, this.DragLayerSelectMode);
                    this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                    this.ViewModel.Layers.ArrangeLayersParents();

                    this.DragSourceLayer = null;
                    this.DragDestinationLayer = null;
                    this.DragLayerSelectMode = SelectMode.None;
                    this.DragLayerOverlayMode = OverlayMode.None;

                    this.ViewModel.Invalidate();//Invalidate
                };
            }
        }


        private void ItemClick(ILayer layer)
        {
            //Is it independent of other layers?
            bool isfreedom = this.KeyboardViewModel.IsCenter;
            //Is select successively?
            bool isLinear = this.KeyboardViewModel.IsSquare;

            //Select a layer.
            if (isfreedom) layer.Selected();
            else if (isLinear) this.ViewModel.Layers.ShiftSelectCurrentLayer(layer);
            else
            {
                foreach (ILayer child in this.ViewModel.Layers.RootLayers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }

                layer.SelectMode = SelectMode.Selected;
                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();//Invalidate
            }

        }


        private async Task AddImage(PickerLocationId location)
        {
            //ImageRe
            ImageRe imageRe = await FileUtil.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images
            ImageRe.DuplicateChecking(imageRe);
            ImageStr imageStr = imageRe.ToImageStr();

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageLayer imageLayer = new ImageLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformerSource),
                StyleManager = new Brushs.StyleManager(transformerSource, transformerSource, imageStr),
            };

            //Selection
            this.SelectionViewModel.SetValue((layer) =>
            {
                layer.SelectMode = SelectMode.UnSelected;
            });

            //Mezzanine
            this.ViewModel.Layers.MezzanineOnFirstSelectedLayer(imageLayer);
            this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();

            this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
            this.ViewModel.Invalidate();//Invalidate
        }

    }
}