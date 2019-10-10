using FanKit.Transformers;
using System.Numerics;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas;
using Windows.UI;
using Windows.UI.Xaml;
using Retouch_Photo2.Layers;
using Retouch_Photo2.Layers.Models;

namespace Retouch_Photo2.TestApp.Pages
{
    public sealed partial class MainPage : Page
    {

        ILayer Layer01 = new GeometryRectangleLayer();
        ILayer Layer02 = new GeometryRectangleLayer();
        ILayer Layer03 = new GeometryRectangleLayer();
        ILayer Layer04 = new GeometryRectangleLayer();

        public MainPage()
        {
            this.InitializeComponent();

            this.StackPanel.Children.Add(this.Layer01.Control.Self);
            this.StackPanel.Children.Add(this.Layer02.Control.Self);
            this.StackPanel.Children.Add(this.Layer03.Control.Self);
            this.StackPanel.Children.Add(this.Layer04.Control.Self);
        }

        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void Button02_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}