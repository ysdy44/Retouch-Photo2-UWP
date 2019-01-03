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
    public sealed partial class ArrangeControl : UserControl
    {
        Layer Layer;

        public ArrangeControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initialize all button
        /// </summary>
        public void Initialize()
        {
            this.Layer = App.ViewModel.CurrentLayer;

            bool isEnabled = !(this.Layer == null);

            //Transform
            this.FlipHorizontalButton.ButtonIsEnabled = isEnabled;
            this.FlipVerticalButton.ButtonIsEnabled = isEnabled;
            this.RotateLeftButton.ButtonIsEnabled = isEnabled;
            this.RotateRightButton.ButtonIsEnabled = isEnabled;
        }

        //Transform
        private void FlipHorizontalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transform((Layer layer) => layer.Transformer.FlipHorizontal = !layer.Transformer.FlipHorizontal);
        private void FlipVerticalButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transform((Layer layer) => layer.Transformer.FlipVertical = !layer.Transformer.FlipVertical);
        private void RotateLeftButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transform((Layer layer) => layer.Transformer.Radian += (float)Math.PI / 2);
        private void RotateRightButton_Tapped(object sender, TappedRoutedEventArgs e) => this.Transform((Layer layer) => layer.Transformer.Radian -= (float)Math.PI / 2);
        private void Transform(Action<Layer> action)
        {
            if (this.Layer == null) return;

            action(this.Layer);

            App.ViewModel.Invalidate();
        }


    }
}
