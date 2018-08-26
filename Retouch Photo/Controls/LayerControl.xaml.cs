using Retouch_Photo.Models;
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

        List<Layer> sdasdsa = new List<Layer>
        {
            new Layer(){Name=  "Layer0",},
            new Layer(){Name=  "Layer1",},
            new Layer(){Name=  "Layer2",},
            new Layer(){Name=  "Layer3",},
        };


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



        private void ListView_ItemClick(object sender, ItemClickEventArgs e){}
        private void CheckBox_Tapped(object sender, TappedRoutedEventArgs e)=>e.Handled = true;

      
    }
}
