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

    public enum BrushType
    {
        None,
        Color,
        Gradient,
        Image,
    }


    public sealed partial class BrushControl : UserControl
    {

        private BrushType brushType;
        public BrushType BrushType
        {
            get => this.brushType;
            set
            {
                this.NoneSegmented.IsChecked = (value == BrushType.None);
                this.ColorSegmented.IsChecked = (value == BrushType.Color);
                this.GradientSegmented.IsChecked = (value == BrushType.Gradient);
                this.ImageSegmented.IsChecked = (value == BrushType.Image);

                this.NoneBorder.Visibility = (value == BrushType.None) ? Visibility.Visible : Visibility.Collapsed;
                this.ColorBorder.Visibility = (value == BrushType.Color) ? Visibility.Visible : Visibility.Collapsed;
                this.GradientBorder.Visibility = (value == BrushType.Gradient) ? Visibility.Visible : Visibility.Collapsed;
                this.ImageBorder.Visibility = (value == BrushType.Image) ? Visibility.Visible : Visibility.Collapsed;

                this.brushType = value;
            }
        }




        public BrushControl()
        {
            this.InitializeComponent();
            this.BrushType = BrushType.None;

            this.NoneSegmented.Tapped += (s, e) => this.BrushType = BrushType.None;
            this.ColorSegmented.Tapped += (s, e) => this.BrushType = BrushType.Color;
            this.GradientSegmented.Tapped += (s, e) => this.BrushType = BrushType.Gradient;
            this.ImageSegmented.Tapped += (s, e) => this.BrushType = BrushType.Image;

        }
    }
}
