using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo.Pickers
{
    /// <summary> Palette Hue</summary>
    public class PaletteHue : PaletteBase
    {
        public PaletteHue()
        {
            this.Name = "Hue";
            this.Unit = "º";
            this.Minimum = 0;
            this.Maximum = 360;
        }

        public override HSL GetHSL(HSL HSL, double value) => new HSL(HSL.A, value, HSL.S, HSL.L);
        public override double GetValue(HSL HSL) => HSL.H;

        public override GradientStopCollection GetSliderBrush(HSL HSL)
        {
            byte A = HSL.A;
            double H = HSL.H;
            double S = HSL.S;
            double L = HSL.L;

            return new GradientStopCollection()
            {
                new GradientStop()
                {
                    Offset = 0,
                    Color = HSL.HSLtoRGB(A, 0, S, L)
                },
                new GradientStop()
                {
                    Offset = 0.16666667,
                    Color = HSL.HSLtoRGB(A, 60, S, L)
                },
                 new GradientStop()
                {
                    Offset = 0.33333333 ,
                    Color = HSL.HSLtoRGB(A, 120, S, L)
                },
                new GradientStop()
                {
                    Offset = 0.5 ,
                    Color = HSL.HSLtoRGB(A, 180, S, L)
                },
                new GradientStop()
                {
                    Offset = 0.66666667 ,
                    Color = HSL.HSLtoRGB(A, 240, S, L)
                },
                new GradientStop()
                {
                    Offset = 0.83333333 ,
                    Color = HSL.HSLtoRGB(A, 300, S, L)
                },
                new GradientStop()
                {
                    Offset = 1 ,
                    Color = HSL.HSLtoRGB(A, 0, S, L)
                },
            };
        }

        public override void Draw(CanvasControl CanvasControl, CanvasDrawingSession ds, HSL HSL, Vector2 Center, float SquareHalfWidth, float SquareHalfHeight)
        {
            //Palette
            Rect rect = new Rect(Center.X - SquareHalfWidth, Center.Y - SquareHalfHeight, SquareHalfWidth * 2, SquareHalfHeight * 2);
            ds.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(CanvasControl, Windows.UI.Colors.White, HSL.HSLtoRGB(HSL.H)) { StartPoint = new Vector2(Center.X - SquareHalfWidth, Center.Y), EndPoint = new Vector2(Center.X + SquareHalfWidth, Center.Y) });
            ds.FillRoundedRectangle(rect, 4, 4, new CanvasLinearGradientBrush(CanvasControl, Windows.UI.Colors.Transparent, Windows.UI.Colors.Black) { StartPoint = new Vector2(Center.X, Center.Y - SquareHalfHeight), EndPoint = new Vector2(Center.X, Center.Y + SquareHalfHeight) });
            ds.DrawRoundedRectangle(rect, 4, 4, Windows.UI.Colors.Gray);

            //Thumb 
            float px = ((float)HSL.S - 50) * SquareHalfWidth / 50 + Center.X;
            float py = (50 - (float)HSL.L) * SquareHalfHeight / 50 + Center.Y;
            HSL.DrawThumb(ds, px, py);
        }
        public override HSL Delta(HSL HSL, Vector2 v, float SquareHalfWidth, float SquareHalfHeight)
        {
            double S = 50 + v.X * 50 / SquareHalfWidth;
            double L = 50 - v.Y * 50 / SquareHalfHeight;

            return new HSL(HSL.A, HSL.H, S, L);
        }
    }
}



