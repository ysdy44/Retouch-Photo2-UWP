using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Retouch_Photo2.TestApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        float value = 0;
        CanvasLinearGradientBrush dasdas;
        private void CanvasControl_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {


            dasdas = CanvasLinearGradientBrush.CreateRainbow(sender, value);
            dasdas.StartPoint = new System.Numerics.Vector2(0, 0);
            dasdas.EndPoint = new System.Numerics.Vector2(200, 0);



            this.Slider.ValueChanged += (s, e) =>
            {
                this.value = (float)e.NewValue;


                dasdas = CanvasLinearGradientBrush.CreateRainbow(CanvasControl, value);
                dasdas.StartPoint = new System.Numerics.Vector2(0, 0);
                dasdas.EndPoint = new System.Numerics.Vector2(200, 0);
                
                this.CanvasControl.Invalidate();





                StringBuilder aaaa=new StringBuilder();
                aaaa.Append(dasdas.Stops.Count());
                aaaa.AppendLine();

                foreach (CanvasGradientStop stop in dasdas.Stops)
                {
                    aaaa.Append($"{stop.Color}  {stop.Position}");
                }
                this.TextBox.Text = aaaa.ToString();
            };
        }

        private void CanvasControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            args.DrawingSession.FillRectangle(new Rect(0,0,200,100),this.dasdas);

        }

    }
}
