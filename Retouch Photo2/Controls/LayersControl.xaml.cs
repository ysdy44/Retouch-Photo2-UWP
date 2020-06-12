using FanKit.Transformers;
using Retouch_Photo2.Elements;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.ViewModels;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Represents a control that arranges <see cref="LayerControl"/> vertically.
    /// </summary>
    public partial class LayersControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        ViewModel SelectionViewModel => App.SelectionViewModel;
        ViewModel MethodViewModel => App.MethodViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel;
        TipViewModel TipViewModel => App.TipViewModel;


        //LayerageCollection
        Layerage DragSourceLayerage;
        Layerage DragDestinationLayerage;
        bool DragLayerIsSelected;
        OverlayMode DragLayerOverlayMode;


        //@Construct
        /// <summary>
        /// Initializes a LayersControl. 
        /// </summary>
        public LayersControl()
        {
            this.InitializeComponent();
            //LayerageCollection
            this.ConstructLayerageCollection();
            this.ItemsControl.ItemsSource = this.ViewModel.LayerageCollection.RootControls;


            this.Tapped += (s, e) => this.MethodViewModel.MethodSelectedNone();//Method
            this.RightTapped += (s, e) => this.ShowLayerMenu();
            this.Holding += (s, e) => this.ShowLayerMenu();


            Retouch_Photo2.PhotosPage.AddCallBack += (photo) =>
            {
                if (photo == null) return;
                
                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer", this.ViewModel.LayerageCollection);
                this.ViewModel.HistoryPush(history);
                
                //Transformer
                Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

                //Layer
                Photocopier photocopier = photo.ToPhotocopier();
                ImageLayer imageLayer = new ImageLayer(this.ViewModel.CanvasDevice)
                {
                    Photocopier = photocopier,
                    IsSelected = true,
                    Transform = new Transform(transformerSource)
                };
                Layerage imageLayerage = imageLayer.ToLayerage();
                LayerBase.Instances.Add(imageLayer);

                //Selection
                this.SelectionViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                });

                //Mezzanine
                LayerageCollection.Mezzanine(this.ViewModel.LayerageCollection, imageLayerage);

                this.SelectionViewModel.SetMode(this.ViewModel.LayerageCollection);//Selection
                LayerageCollection.ArrangeLayers(this.ViewModel.LayerageCollection);
                this.ViewModel.Invalidate();//Invalidate
            };
        }

        private void ShowLayerMenu()
        {
            this.TipViewModel.ShowMenuLayout(MenuType.Layer);
        }
        private void ShowLayerMenu(Layerage layerage)
        {
            ILayer layer = layerage.Self;

            this.TipViewModel.ShowMenuLayoutAt(MenuType.Layer, layer.Control.Self, FlyoutPlacementMode.Left);
        }

    }
}