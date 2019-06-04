using Retouch_Photo2.Models;
using Retouch_Photo2.Models.Layers;
using Retouch_Photo2.ViewModels;
using System;
using System.Numerics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Controls
{
    public sealed partial class LayersControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo2.App.ViewModel;
        
        //Delegate
        public delegate void FlyoutShowHandler(FrameworkElement placementTarget);
        public event FlyoutShowHandler FlyoutShow = null;
        
        public LayersControl()
        {
            this.InitializeComponent();

            Layer.LayerItemClick += (layer, placementTarget) =>
            {
                if (layer.IsChecked)
                {
                    if (this.control == placementTarget)
                    {
                        this.FlyoutShow?.Invoke(placementTarget);//Delegate
                    }
                    else this.control = placementTarget;
                }
                else
                {
                    layer.IsChecked = true;

                    this.ViewModel.RenderLayer.Selected(layer);
                    this.ViewModel.SetLayer(layer);

                    this.ViewModel.Invalidate();
                }

                this.ViewModel.RenderLayer.Selected(layer);
            };
            /*
                         this.ListView.IsItemClickEnabled = true;
                        this.ListView.ItemClick += (s, e) =>
                        {
                            if (e.ClickedItem is Layer layer)
                            {
                                this.ViewModel.SetLayer(layer);
                            }
                            this.ViewModel.Invalidate();
                        };
                        this.ListView.SelectionChanged+=(s,e) => this.ViewModel.Invalidate();
                         */


            this.AddButton.Tapped += async (s, e) =>
             { 
                FileOpenPicker openPicker = new FileOpenPicker
                 {
                     ViewMode = PickerViewMode.Thumbnail,
                     SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                     FileTypeFilter =
                 {
                     ".jpg",
                     ".jpeg",
                     ".png",
                     ".bmp",
                 }
                 };

                 StorageFile file = await openPicker.PickSingleFileAsync();
                 if (file == null) return;

                 Vector2 center = this.ViewModel.MatrixTransformer.Center;
                 Layer layer = await ImageLayer.CreateFromFlie(this.ViewModel.CanvasDevice, file, center);

                 this.ViewModel.RenderLayer.Insert(layer);
                 this.ViewModel.SetLayer(layer);

                 this.ViewModel.Invalidate();
             };
        }

        //Flyout
        FrameworkElement control;
        /*
        private void LayoutControl_FlyoutShow(UserControl control, Layer layer, bool isShow)
        {
            if (this.control == control || isShow)
            {
                this.FlyoutShow?.Invoke(control);//Delegate
            }
            else this.control = control;
        }*/

        //Layer
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.Invalidate();
        }
    }
}
