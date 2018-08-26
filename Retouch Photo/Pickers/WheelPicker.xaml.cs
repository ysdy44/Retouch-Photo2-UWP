using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo.Pickers
{
    public sealed partial class WheelPicker : UserControl
    {

        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChange = null;


        #region DependencyProperty
        

        private Color color = Color.FromArgb(255, 255, 255, 255);
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                this.HSL = HSL.RGBtoHSL(value);
            }
        }


        public HSL HSL
        {
            get { return (HSL)GetValue(HSLProperty); }
            set { SetValue(HSLProperty, value); }
        }
        public static readonly DependencyProperty HSLProperty = DependencyProperty.Register(nameof(HSL), typeof(HSL), typeof(WheelPicker), new PropertyMetadata(new HSL(255, 360, 100, 100), new PropertyChangedCallback(HSLOnChanged)));
        private static void HSLOnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            WheelPicker con = (WheelPicker)sender;

            if (e.NewValue is HSL NewValue) con.HSLChanged(NewValue);
        }
        private void HSLChanged(HSL value)
        {
            byte A = value.A;
            double H = value.H;
            double S = value.S;
            double L = value.L;

            this.color = HSL.HSLtoRGB(A, H, S, L);
            this.ColorChange?.Invoke(this, this.color);

            this.CanvasControl.Invalidate();
        }

         

        public SolidColorBrush Stroke
        {
            get { return (SolidColorBrush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        public static readonly DependencyProperty StrokeProperty =DependencyProperty.Register(nameof(Stroke), typeof(SolidColorBrush), typeof(WheelPicker), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Gray)));


        #endregion

        
        Vector2 Center = new Vector2(50, 50);
        float Radio = 100;

        float StrokeWidth = 8;
        float SquareRadio => (this.Radio - this.StrokeWidth) / 1.414213562373095f;


        public WheelPicker()
        {
            this.InitializeComponent();
        }


        private void CanvasControl_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            this.Center = e.NewSize.ToVector2() / 2;

            this.Radio = (float)Math.Min(e.NewSize.Width, e.NewSize.Height) / 2 - this.StrokeWidth;
        }
        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            //Wheel           
            args.DrawingSession.DrawCircle(this.Center, this.Radio , this.Stroke.Color,this.StrokeWidth*2);
            for (float angle = 0; angle < (float)Math.PI * 2; angle += (float)(2 * Math.PI) / (int)(Math.PI * Radio * 2 / this.StrokeWidth)) args.DrawingSession.FillCircle((float)Math.Cos(angle) * this.Radio + this.Center.X, (float)Math.Sin(angle) * this.Radio + this.Center.Y, this.StrokeWidth, HSL.HSLtoRGB(((angle * 180.0 / Math.PI) + 360.0) % 360.0));
            args.DrawingSession.DrawCircle(this.Center, this.Radio - this.StrokeWidth, this.Stroke.Color);
            args.DrawingSession.DrawCircle(this.Center, this.Radio + this.StrokeWidth, this.Stroke.Color);

            //Thumb
            double ang = (float)(((this.HSL.H + 360.0) % 360.0) * Math.PI / 180.0);
            float wx = (float)Math.Cos(ang) * this.Radio + this.Center.X;
            float wy = (float)Math.Sin(ang) * this.Radio + this.Center.Y;
            args.DrawingSession.DrawCircle(wx, wy, 8, Windows.UI.Colors.Black, 4);
            args.DrawingSession.DrawCircle(wx, wy, 8, Windows.UI.Colors.White, 2);


            //Palette
            Rect rect = new Rect(this.Center.X - this.SquareRadio, this.Center.Y - this.SquareRadio, this.SquareRadio * 2, this.SquareRadio * 2);
            args.DrawingSession.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(this.CanvasControl, Windows.UI.Colors.White, HSL.HSLtoRGB(this.HSL.H)) { StartPoint = new Vector2(this.Center.X - this.SquareRadio, this.Center.Y), EndPoint = new Vector2(this.Center.X + this.SquareRadio, this.Center.Y) });
            args.DrawingSession.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(this.CanvasControl, Windows.UI.Colors.Transparent, Windows.UI.Colors.Black) { StartPoint = new Vector2(this.Center.X, this.Center.Y - this.SquareRadio), EndPoint = new Vector2(this.Center.X, this.Center.Y + this.SquareRadio) });
            args.DrawingSession.DrawRoundedRectangle(rect, 4, 4, this.Stroke.Color);

            //Thumb 
            float px = ((float)this.HSL.S - 50) * this.SquareRadio / 50 + this.Center.X;
            float py = (50 - (float)this.HSL.L) * this.SquareRadio / 50 + this.Center.Y;
            args.DrawingSession.DrawCircle(px, py, 8, Windows.UI.Colors.Black, 4);
            args.DrawingSession.DrawCircle(px, py, 8, Windows.UI.Colors.White, 2);
        }



        bool IsWheel = false;
        bool IsPalette = false;
        Vector2 v;
        private void CanvasControl_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            v = e.Position.ToVector2() - this.Center;

            this.IsWheel = v.Length() + this.StrokeWidth > this.Radio && v.Length() - this.StrokeWidth < this.Radio;
            this.IsPalette = Math.Abs(v.X) < this.SquareRadio && Math.Abs(v.Y) < this.SquareRadio;
        }
        private void CanvasControl_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (this.IsWheel|| this.IsPalette)
            {
                v += e.Delta.Translation.ToVector2();

                double H = (((Math.Atan2(v.Y, v.X)) * 180.0 / Math.PI) + 360.0) % 360.0;
                double S = v.X  * 50 / this.SquareRadio + 50;
                double L = 50 - v.Y   * 50 / this.SquareRadio;

                this.HSL = new HSL(this.HSL.A,this.IsWheel ? H : this.HSL.H,this.IsPalette ? S : this.HSL.S,this.IsPalette ? L : this.HSL.L);
            }
        }
        private void CanvasControl_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e) => this.IsWheel = this.IsPalette = false;

        
    }
}
