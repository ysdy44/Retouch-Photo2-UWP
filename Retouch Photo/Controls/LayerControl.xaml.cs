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
        public DrawViewModel ViewModel;

        public LayerControl()
        {
            this.InitializeComponent();

            //ViewModel
            this.ViewModel = App.ViewModel;
        } 



        private Grid element;
        private void Grid_Holding(object sender, HoldingRoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout((Grid)sender);
        private void Grid_RightTapped(object sender, RightTappedRoutedEventArgs e) => FlyoutBase.ShowAttachedFlyout((Grid)sender);
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Grid element)
            {
                if (this.element == element)
                {
                    FlyoutBase.ShowAttachedFlyout(element);
                }
            }
            this.element = (Grid)sender;
        }



        private void ListView_ItemClick(object sender, ItemClickEventArgs e){}


        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate(isRenderLayerRender: true);
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ViewModel.Invalidate(isRenderLayerRender: true);
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.Invalidate(isRenderLayerRender: true);
        }


        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.ViewModel.RenderLayer.Layers==null)
            {
                this.ViewModel.Text = "Layer is null";
            }
        }

    }
}
