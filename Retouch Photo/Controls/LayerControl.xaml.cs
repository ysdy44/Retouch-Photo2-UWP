using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.Models.Layers;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Controls
{
    public sealed partial class LayerControl : UserControl
    {

        //ViewModel
        public DrawViewModel ViewModel => App.ViewModel;

        public LayerControl()
        {
            this.InitializeComponent();
        }

        //Flyout
        UserControl control;
        private void LayerLayoutControl_FlyoutShow(UserControl control, Layer layer, bool isShow)
        {
            if (this.control == control || isShow)
            {
                this.LayerFlyoutControl.Layer = layer;
                this.PropertyFlyout.ShowAt(control);
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
                this.ViewModel.CurrentLayer = item;
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
            Layer layer = await ImageLayer.CreateFromFlie(this.ViewModel.CanvasControl, file);

            layer.Transformer.Postion = this.ViewModel.MatrixTransformer.ControlToVirtualToCanvasCenter - new Vector2(layer.Transformer.Width, layer.Transformer.Height) / 2;

            this.ViewModel.RenderLayer.Insert(layer);
            this.ViewModel.Invalidate();
        }


    }
}
