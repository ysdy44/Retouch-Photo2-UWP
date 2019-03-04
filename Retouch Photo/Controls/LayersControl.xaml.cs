using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.ViewModels;
using System;
using System.Numerics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo.Controls
{
    public sealed partial class LayersControl : UserControl
    {

        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;
        
        //Delegate
        public delegate void FlyoutShowHandler(UserControl control);
        public event FlyoutShowHandler FlyoutShow = null;


        public LayersControl()
        {
            this.InitializeComponent();
        }


        //Flyout
        UserControl control;
        private void LayoutControl_FlyoutShow(UserControl control, Layer layer, bool isShow)
        {
            if (this.control == control || isShow)
            {
                this.FlyoutShow?.Invoke(control);//Delegate
            }
            else this.control = control;
        }


        //Layer
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.Invalidate();
        }


        //ListView
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ViewModel.Invalidate();
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Layer item)
            {
                this.ViewModel.CurrentLayer=item;
            }
            this.ViewModel.Invalidate();
        }


        private async void AddButton_Tapped(object sender, TappedRoutedEventArgs e)
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
            Layer layer = await ImageLayer.CreateFromFlie(this.ViewModel.CanvasDevice, file);

            layer.Transformer.Postion = this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasCenter - new Vector2(layer.Transformer.Width, layer.Transformer.Height) / 2;

            this.ViewModel.RenderLayer.Insert(layer);
            this.ViewModel.Invalidate();
        }
    }
}
