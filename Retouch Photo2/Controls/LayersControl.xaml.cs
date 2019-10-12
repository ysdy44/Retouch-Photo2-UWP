using FanKit.Transformers;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Linq;
using System.Threading.Tasks;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public partial class LayersControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SelectionViewModel SelectionViewModel => App.SelectionViewModel;
        KeyboardViewModel KeyboardViewModel => App.KeyboardViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //@Content
        /// <summary> IndicatorBorder. </summary>
        public Border IndicatorBorder => this._IndicatorBorder; 
        /// <summary> WidthButton. </summary>
        public Button WidthButton => this._WidthButton;


        //LayerCollection
        ILayer DragSourceLayer;
        ILayer DragDestinationLayer;
        SelectMode DragLayerSelectMode;
        OverlayMode DragLayerOverlayMode;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();
            this.ItemsControl.ItemsSource = this.ViewModel.Layers.RootControls;


            #region Add


            this.HeightSlider.ValueChanged += (s, e) =>
            {
                if (e.NewValue == e.OldValue) return;
                int controlHeight = (int)e.NewValue;
                this.ViewModel.Layers.SetControlHeight(controlHeight);
            };

            this.PhotoButton.Tapped += async (s, e) => await this.AddImage(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += async (s, e) => await this.AddImage(PickerLocationId.Desktop);

            this.LayoutButton.Tapped += (s, e) => this.ShowLayerMenu();


            #endregion


            #region This


            this.Tapped += (s, e) =>
            {
                foreach (ILayer child in this.ViewModel.Layers.RootLayers)
                {
                    child.SelectMode = SelectMode.UnSelected;
                }

                this.SelectionViewModel.SetModeNone();//Selection
                this.ViewModel.Invalidate();
            };
            this.RightTapped += (s, e) => this.ShowLayerMenu();
            this.Holding += (s, e) => this.ShowLayerMenu();
            this.AddButton.Tapped += (s, e) =>
            {
                this.AddImageFlyout.ShowAt(this.AddButton);
                e.Handled = true;
            };


            #endregion


            #region LayerCollection


            LayerCollection.ItemClick += (layer) =>
            {
                if (layer.SelectMode == SelectMode.Selected)
                {
                    this.ShowLayerMenu(layer);
                    return;
                }

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
                    this.ViewModel.Invalidate();
                }
            };
            LayerCollection.RightTapped += (layer) => this.ShowLayerMenu(layer);
            LayerCollection.SelectChanged += () =>
            {
                this.SelectionViewModel.SetMode(this.ViewModel.Layers);//Selection
                this.ViewModel.Invalidate();
            };


            LayerCollection.DragItemsStarted += (layer, selectMode) =>
            {
                this.DragSourceLayer = layer;
                this.DragLayerSelectMode = selectMode;
            };
            LayerCollection.DragItemsDelta += (layer, overlayMode) =>
            {
                this.DragDestinationLayer = layer;
                this.DragLayerOverlayMode = overlayMode;
            };
            LayerCollection.DragItemsCompleted += () =>
            {
                this.ViewModel.Layers.DragComplete(this.DragDestinationLayer, this.DragSourceLayer, this.DragLayerOverlayMode, this.DragLayerSelectMode);
                this.ViewModel.Layers.ArrangeLayersControlsWithClearAndAdd();
                this.ViewModel.Layers.ArrangeLayersParents();

                this.DragSourceLayer = null;
                this.DragDestinationLayer = null;
                this.DragLayerSelectMode = SelectMode.None;
                this.DragLayerOverlayMode = OverlayMode.None;
            };


            #endregion
        }

        private void ShowLayerMenu()
        {
            this.TipViewModel.SetMenuState(MenuType.Layer, MenuState.FlyoutHide, MenuState.FlyoutShow);
        }
        private void ShowLayerMenu(ILayer layer)
        {
            Point rootGridPosition = layer.Control.Self.TransformToVisual(this).TransformPoint(new Point());
            Canvas.SetTop(this._IndicatorBorder, rootGridPosition.Y);

            this.ShowLayerMenu();
        }

               
        private async Task AddImage(PickerLocationId location)
        {
            //ImageRe
            ImageRe imageRe = await ImageRe.CreateFromLocationIdAsync(this.ViewModel.CanvasDevice, location);
            if (imageRe == null) return;

            //Images
            this.ViewModel.DuplicateChecking(imageRe);

            //Transformer
            Transformer transformerSource = new Transformer(imageRe.Width, imageRe.Height, Vector2.Zero);

            //Layer
            ImageLayer imageLayer = new ImageLayer
            {
                SelectMode = SelectMode.Selected,
                TransformManager = new TransformManager(transformerSource),

                ImageRe = imageRe,
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