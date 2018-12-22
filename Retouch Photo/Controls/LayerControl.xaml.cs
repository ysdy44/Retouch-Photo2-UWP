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

        
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Layer item)
            {
                this.ViewModel.RenderLayer.CurrentLayer = item;
                this.ViewModel.Invalidate();
            }
        }


        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e) => this.ViewModel.Invalidate();
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => this.ViewModel.Invalidate();
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.ViewModel.Invalidate();
        }


        private void AddButton_Tapped(object sender, TappedRoutedEventArgs e)
        {     

        }
     
    }
}
