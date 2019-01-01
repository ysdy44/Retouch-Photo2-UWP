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

namespace Retouch_Photo.Pages.ToolPages
{
    public sealed partial class ToolCursorPage : Page
    {
        //ViewModel
        DrawViewModel ViewModel => App.ViewModel;

        public ToolCursorPage()
        {
            this.InitializeComponent();
        }

        private void StepFrequencyButton_Tapped(object sender, TappedRoutedEventArgs e) => this.ViewModel.Invalidate();
        private void SkewButton_Tapped(object sender, TappedRoutedEventArgs e) => this.ViewModel.Invalidate();
                     
        private void FlipHorizontalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transformer((Layer layer) => layer.Transformer.FlipHorizontal = !layer.Transformer.FlipHorizontal);
        private void FlipVerticalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transformer((Layer layer) => layer.Transformer.FlipVertical = !layer.Transformer.FlipVertical);
        private void LeftTurnButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transformer((Layer layer) => layer.Transformer.Radian += (float)Math.PI / 2);
        private void RightTurnButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transformer((Layer layer) => layer.Transformer.Radian -= (float)Math.PI / 2);
        private void Transformer(Action<Layer> action)
        {
            Layer layer = App.ViewModel.CurrentLayer;
            if (layer == null) return;

            action(layer);

            App.ViewModel.Invalidate();
        }


    }
}
