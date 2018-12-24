using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        private void LayerLayoutControl_FlyoutShow(UserControl control, Layer layer)
        {
            this.LayerPropertyControl.Layer = layer;
            this.PropertyFlyout.ShowAt(control);
        }
               

        //ListView
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ViewModel.Invalidate();
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Layer item)
            {
                this.ViewModel.RenderLayer.CurrentLayer = item;
                this.ViewModel.Invalidate();
            }
        }
        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }
        

        //Layer
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.Invalidate();
        }
    }
}
