using FanKit.Transformers;
using Retouch_Photo2.Historys;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;
using Retouch_Photo2.Menus;
using Retouch_Photo2.Photos;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2
{
    public sealed partial class DrawPage : Page
    {

        //LayerManager
        Layerage DragSourceLayerage;
        Layerage DragDestinationLayerage;
        bool DragLayerIsSelected;
        OverlayMode DragLayerOverlayMode;


        //LayersControl
        private void ConstructLayersControl()
        {
            this.LayersScrollViewer.Content = LayerageCollection.StackPanel;

            this.LayersScrollViewer.Tapped += (s, e) => this.MethodViewModel.MethodSelectedNone();//Method
            this.LayersScrollViewer.RightTapped += (s, e) => this.ShowLayerMenu();
            this.LayersScrollViewer.Holding += (s, e) => this.ShowLayerMenu();


            Retouch_Photo2.PhotosPage.AddImageCallBack += (photo) =>
            {
                if (photo == null) return;

                //History
                LayeragesArrangeHistory history = new LayeragesArrangeHistory("Add layer");
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
                LayerageCollection.Mezzanine(imageLayerage);

                this.SelectionViewModel.SetMode();//Selection
                LayerageCollection.ArrangeLayers();
                LayerageCollection.ArrangeLayersBackground();
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

            this.TipViewModel.ShowMenuLayoutAt(MenuType.Layer, layer.Control, FlyoutPlacementMode.Left);
        }

    }
}