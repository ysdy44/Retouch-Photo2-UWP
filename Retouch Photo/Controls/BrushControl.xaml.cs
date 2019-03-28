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
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Text;
using Windows.Foundation.Metadata;
using System.Globalization;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Retouch_Photo.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Retouch_Photo.ViewModels;

namespace Retouch_Photo.Controls
{
    public enum BrushControlState
    {
        None,
        Color,
        Gradient,
        Image,
    }

    //Delegate
    public delegate void CanvasBrushHandler(ICanvasBrush brush);

    public sealed partial class BrushControl : UserControl
    {
        //ViewModel
        public DrawViewModel ViewModel => Retouch_Photo.App.ViewModel;

        public event CanvasBrushHandler hh;

        private BrushControlState state;
        public BrushControlState State
        {
            get => this.state;
            set
            {
                this.NoneSegmented.IsChecked = (value == BrushControlState.None);
                this.ColorSegmented.IsChecked = (value == BrushControlState.Color);
                this.GradientSegmented.IsChecked = (value == BrushControlState.Gradient);
                this.ImageSegmented.IsChecked = (value == BrushControlState.Image);

                this.NoneBorder.Visibility = (value == BrushControlState.None) ? Visibility.Visible : Visibility.Collapsed;
                this.ColorBorder.Visibility = (value == BrushControlState.Color) ? Visibility.Visible : Visibility.Collapsed;
                this.GradientBorder.Visibility = (value == BrushControlState.Gradient) ? Visibility.Visible : Visibility.Collapsed;
                this.ImageBorder.Visibility = (value == BrushControlState.Image) ? Visibility.Visible : Visibility.Collapsed;

                this.state = value;
            }
        }
         


        CanvasGradientStop[] gradientStops = new CanvasGradientStop[] 
        {
        };

        public BrushControl()
        {
            this.InitializeComponent();
            this.State = BrushControlState.None;

            this.NoneSegmented.Tapped += (s, e) => this.State = BrushControlState.None;
            this.ColorSegmented.Tapped += (s, e) => this.State = BrushControlState.Color;
            this.GradientSegmented.Tapped += (s, e) => this.State = BrushControlState.Gradient;
            this.ImageSegmented.Tapped += (s, e) => this.State = BrushControlState.Image;

            // Matrix
            this.Button1.Tapped += (s, e) =>
            {
                CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(this.ViewModel.CanvasDevice, Colors.Gray, Colors.White)
                {
                    StartPoint = Vector2.Zero,
                    EndPoint = Vector2.One * 100
                };
                this.ViewModel.CurrentLayer.BrushChanged(brush);
                this.ViewModel.Invalidate();
            };
            this.Button2.Tapped += (s, e) =>
            {
                CanvasLinearGradientBrush brush = new CanvasLinearGradientBrush(this.ViewModel.CanvasDevice, Colors.Gray, Colors.Black)
                {
                    StartPoint = Vector2.Zero,
                    EndPoint = Vector2.One * 100
                };
                this.ViewModel.CurrentLayer.BrushChanged(brush);
                this.ViewModel.Invalidate();
            };
            this.Button3.Tapped += (s, e) =>
            {
                CanvasLinearGradientBrush brush = CanvasLinearGradientBrush.CreateRainbow(this.ViewModel.CanvasDevice, 0);
                brush.StartPoint = Vector2.Zero;
                brush.EndPoint = Vector2.One * 100;
                this.ViewModel.CurrentLayer.BrushChanged(brush);
                this.ViewModel.Invalidate();
            };
            this.Button4.Tapped += (s, e) =>
            {
                CanvasLinearGradientBrush brush = CanvasLinearGradientBrush.CreateRainbow(this.ViewModel.CanvasDevice, 1);
                brush.StartPoint = Vector2.Zero;
                brush.EndPoint = Vector2.One * 100;
                this.ViewModel.CurrentLayer.BrushChanged(brush);
                this.ViewModel.Invalidate();
            };
        }



    }
}
