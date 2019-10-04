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
        /// <summary> IndicatorBorder's Child. </summary>
        public UIElement IndicatorChild { get => this.IndicatorBorder.Child; set => this.IndicatorBorder.Child = value; }
        /// <summary> AddButton </summary>
        public Button AddButton => this._AddButton;
        
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

            //Slider
            this.ControlHeightSlider.ValueChanged += (s, e) =>
            {
                if (e.NewValue == e.OldValue) return;
                int controlHeight = (int)e.NewValue;
                this.ViewModel.Layers.ControlsHeight = controlHeight;
            };

            #region LayerCollection

            this.ViewModel.Layers.ItemClick += (layer) =>
            {             
                if (layer.SelectMode.ToBool())
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
                }
            };
            this.ViewModel.Layers.RightTapped += (layer) =>
            {
                this.ShowLayerMenu(layer);
            };

            this.ViewModel.Layers.DragItemsStarted += (layer, selectMode) =>
            {
                this.DragSourceLayer = layer;
                this.DragLayerSelectMode = selectMode;
            };
            this.ViewModel.Layers.DragItemsDelta += (layer, overlayMode) =>
            {
                this.DragDestinationLayer = layer;
                this.DragLayerOverlayMode = overlayMode;
            };
            this.ViewModel.Layers.DragItemsCompleted += () =>
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


        private void ShowLayerMenu(ILayer layer)
        {
            Point rootGridPosition = layer.Control.Self.TransformToVisual(this).TransformPoint(new Point());
            Canvas.SetTop(this.IndicatorBorder, rootGridPosition.Y);

            //Menu
            this.TipViewModel.SetMenuState(MenuType.Layer, MenuState.FlyoutHide, MenuState.FlyoutShow);
        }
                 
    }
}