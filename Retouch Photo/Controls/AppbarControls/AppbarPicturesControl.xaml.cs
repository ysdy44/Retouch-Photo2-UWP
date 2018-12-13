using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo.Controls.AppbarControls
{
    public sealed partial class AppbarPicturesControl : UserControl
    {

        //Delegate
        public delegate void PicturesEventHandler(PickerLocationId location);
        public event PicturesEventHandler PicturesPicker;

        public event TappedEventHandler CancelButtonTapped;

        public AppbarPicturesControl()
        {
            this.InitializeComponent();
        }

        private void PhotoButton_Tapped(object sender, TappedRoutedEventArgs e) => this.PicturesPicker?.Invoke(PickerLocationId.PicturesLibrary);
        private void DestopButton_Tapped(object sender, TappedRoutedEventArgs e) => this.PicturesPicker?.Invoke(PickerLocationId.Desktop);

        private void CancelButton_Tapped(object sender, TappedRoutedEventArgs e) => this.CancelButtonTapped?.Invoke(sender, e);


    }
}
