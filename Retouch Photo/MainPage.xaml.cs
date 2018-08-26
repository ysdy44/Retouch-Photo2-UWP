using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        


        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
           this.Frame.Navigate(typeof(Retouch_Photo.DrawPage));//Main:Navigate
        }

        private void PopupButton_Tapped(object sender, TappedRoutedEventArgs e) => this.AndroidDialog.IsShow = true;
        private void Button_Tapped(object sender, TappedRoutedEventArgs e) => this.GameDialog.IsShow = true;

    }
}
