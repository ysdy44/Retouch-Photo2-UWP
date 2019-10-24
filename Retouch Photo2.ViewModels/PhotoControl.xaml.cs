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

namespace Retouch_Photo2.ViewModels
{
    public sealed partial class PhotoControl : UserControl
    {


        public void SetSelectMode(bool? value)
        {
            switch (value)
            {
                case null:
                    this.RootGrid.Background = this.UnAccentColor;
                    this.BackgroundGrid.BorderThickness = new Thickness(0);

                    this.SelectedBorder.Visibility = Visibility.Collapsed;
                    break;
                case false:
                    this.RootGrid.Background = this.UnAccentColor;
                    this.BackgroundGrid.BorderThickness = new Thickness(1);

                    this.SelectedBorder.Visibility = Visibility.Visible;
                    this.SelectedBorder.Background = this.UnCheckColor;
                    break;
                case true:
                    this.RootGrid.Background = this.AccentColor;
                    this.BackgroundGrid.BorderThickness = new Thickness(1);
                    
                    this.SelectedBorder.Visibility = Visibility.Visible;
                    this.SelectedBorder.Background = this.CheckColor;
                    break;
            }
        }


        //@Construct
        public PhotoControl(Photo photo)
        {
            this.InitializeComponent();

            this.TextBlock.Text = photo.Name;
            this.ImageEx.Source = photo.Uri;

            this.RootGrid.Tapped += (s, e) =>
            {
                Photo.ItemClick?.Invoke(this.BackgroundGrid, photo);//Delegate
            };
        }

    }
}