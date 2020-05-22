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
using Retouch_Photo2.Elements;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo2.Controls
{
    /// <summary> 
    /// Retouch_Photo2's the only <see cref = "LayersControl" />. 
    /// </summary>
    public partial class LayersControl : UserControl
    {
        //@ViewModel
        ViewModel ViewModel => App.ViewModel;
        SettingViewModel SettingViewModel => App.SettingViewModel ;
        TipViewModel TipViewModel => App.TipViewModel;


        //LayerCollection
        Layerage DragSourceLayer;
        Layerage DragDestinationLayer;
        bool DragLayerIsSelected;
        OverlayMode DragLayerOverlayMode;


        //@Construct
        public LayersControl()
        {
            this.InitializeComponent();
            //LayerCollection
            this.ConstructLayerCollection();
            this.ItemsControl.ItemsSource = this.ViewModel.LayerCollection.RootControls;


            this.Tapped += (s, e) =>
            {
                this.ViewModel.SetModeNone();//Selection
                LayerCollection.ArrangeLayersBackgroundLayerCollection(this.ViewModel.LayerCollection);
                this.ViewModel.Invalidate();
            };
            this.RightTapped += (s, e) => this.ShowLayerMenu();
            this.Holding += (s, e) => this.ShowLayerMenu();


            Retouch_Photo2.PhotosPage.AddCallBack += (photo) =>
            {
                if (photo == null) return;


                //History
                this.ViewModel.HistoryPushLayeragesHistory("Add layer");


                //Transformer
                Transformer transformerSource = new Transformer(photo.Width, photo.Height, Vector2.Zero);

                //Layer
                Photocopier photocopier = photo.ToPhotocopier();
                ImageLayer imageLayer = new ImageLayer(transformerSource, photocopier)
                {
                    IsSelected = true,
                    Transform = new Transform(transformerSource)
                };
                Layer.Instances.Add(imageLayer);
                Layerage imageLayerage = imageLayer.ToLayerage();

                //Selection
                this.ViewModel.SetValue((layerage) =>
                {
                    ILayer layer = layerage.Self;

                    layer.IsSelected = false;
                });

                //Mezzanine
                LayerCollection.Mezzanine(this.ViewModel.LayerCollection, imageLayerage);

                this.ViewModel.SetMode(this.ViewModel.LayerCollection);//Selection
                LayerCollection.ArrangeLayersControls(this.ViewModel.LayerCollection);
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