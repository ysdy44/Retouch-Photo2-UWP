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

namespace Retouch_Photo2.TestApp.Pages
{

    public interface IMenu
    {

    }

    public sealed partial class MainPage : Page
    {  
        public MainPage()
        {
            this.InitializeComponent();
        }




        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(MainPage), new PropertyMetadata(false));



        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            IsOpen = !IsOpen;
        }
    }
}