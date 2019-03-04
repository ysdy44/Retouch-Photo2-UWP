using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pickers
{
    //Delegate
    public delegate void AlphaChangeHandler(object sender, byte value);
    public delegate void ColorChangeHandler(object sender, Color value);

    /// <summary> Color form HSL </summary>
    public struct HSL
    {

        /// <summary> Alpha </summary>
        public byte A;

        /// <summary> Hue </summary>
        public double H
        {
            get => h;
            set
            {
                if (value < 0) h = 0;
                else if (value > 360) h = 360;
                else h = value;
            }
        }
        private double h;

        /// <summary> Saturation </summary>
        public double S
        {
            get => s;
            set
            {
                if (value < 0) s = 0;
                else if (value > 100) s = 100;
                else s = value;
            }
        }
        private double s;

        /// <summary> Lightness </summary>
        public double L
        {
            get => l;
            set
            {
                if (value < 0) l = 0;
                else if (value > 100) l = 100;
                else l = value;
            }
        }
        private double l;



        public HSL(byte A, double H, double S, double L) { this.A = A; this.h = H; this.s = L; this.l = L; this.H = H; this.S = S; this.L = L; }



        /// <summary> RGB to HSL </summary>
        /// <param name="H"> Hue </param>
        /// <returns> Color </returns>
        public static Color HSLtoRGB(double H)
        {
            double hh = H / 60;
            byte xhh = (byte)((1 - Math.Abs(hh % 2 - 1)) * 255);

            if (hh < 1) return Color.FromArgb(255, 255, xhh, 0);
            else if (hh < 2) return Color.FromArgb(255, xhh, 255, 0);
            else if (hh < 3) return Color.FromArgb(255, 0, 255, xhh);
            else if (hh < 4) return Color.FromArgb(255, 0, xhh, 255);
            else if (hh < 5) return Color.FromArgb(255, xhh, 0, 255);
            else return Color.FromArgb(255, 255, 0, xhh);
        }

        /// <summary> RGB to HSL </summary>
        /// <param name="hsl"> HSL </param>
        /// <returns> Color </returns>
        public static Color HSLtoRGB(HSL hsl) => HSL.HSLtoRGB(hsl.A, hsl.H, hsl.S, hsl.L);

        /// <summary> RGB to HSL </summary>
        /// <param name="a"> Alpha </param>
        /// <param name="h"> Hue </param>
        /// <param name="s"> Saturation </param>
        /// <param name="l"> Lightness </param>
        /// <returns> Color </returns>
        public static Color HSLtoRGB(byte a, double h, double s, double l)
        {
            if (h == 360) h = 0;

            if (s == 0)
            {
                byte ll = (byte)(l / 100 * 255);
                return Color.FromArgb(a, ll, ll, ll);
            }

            double S = s / 100;
            double V = l / 100;

            int H1 = (int)(h * 1.0f / 60);
            double F = h / 60 - H1;
            double P = V * (1.0f - S);
            double Q = V * (1.0f - F * S);
            double T = V * (1.0f - (1.0f - F) * S);

            double R = 0f, G = 0f, B = 0f;
            switch (H1)
            {
                case 0: R = V; G = T; B = P; break;
                case 1: R = Q; G = V; B = P; break;
                case 2: R = P; G = V; B = T; break;
                case 3: R = P; G = Q; B = V; break;
                case 4: R = T; G = P; B = V; break;
                case 5: R = V; G = P; B = Q; break;
            }

            R = R * 255;
            while (R > 255) R -= 255;
            while (R < 0) R += 255;

            G = G * 255;
            while (G > 255) G -= 255;
            while (G < 0) G += 255;

            B = B * 255;
            while (B > 255) B -= 255;
            while (B < 0) B += 255;

            return Color.FromArgb(a, (byte)R, (byte)G, (byte)B);
        }



        /// <summary> RGB to HSL </summary>
        /// <param name="color"> Color </param>
        /// <returns> HSL </returns>
        public static HSL RGBtoHSL(Color color) => HSL.RGBtoHSL(color.A, color.R, color.G, color.B);

        /// <summary> RGB to HSL </summary>
        /// <param name="a"> Alpha </param>
        /// <param name="r"> Red </param>
        /// <param name="g"> Green </param>
        /// <param name="b"> Blue </param>
        /// <returns> HSL </returns>
        public static HSL RGBtoHSL(byte a, byte r, byte g, byte b)
        {
            double R = r * 1.0f / 255;
            double G = g * 1.0f / 255;
            double B = b * 1.0f / 255;

            double min = Math.Min(Math.Min(R, G), B);
            double max = Math.Max(Math.Max(R, G), B);

            double H = 0, S, V;

            if (max == min) { H = 0; }

            else if (max == R && G > B) H = 60 * (G - B) * 1.0f / (max - min) + 0;
            else if (max == R && G < B) H = 60 * (G - B) * 1.0f / (max - min) + 360;
            else if (max == G) H = H = 60 * (B - R) * 1.0f / (max - min) + 120;
            else if (max == B) H = H = 60 * (R - G) * 1.0f / (max - min) + 240;

            if (max == 0) S = 0;
            else S = (max - min) * 1.0f / max;

            V = max;

            return new HSL(a, H, (S * 100), V * 100);
        }


        /// <summary> Draw a ⊙. </summary>
        public static void DrawThumb(CanvasDrawingSession ds, Vector2 vector) => HSL.DrawThumb(ds, vector.X, vector.Y);
        public static void DrawThumb(CanvasDrawingSession ds, float px, float py)
        {
            ds.DrawCircle(px, py, 9, Windows.UI.Colors.Black, 5);
            ds.DrawCircle(px, py, 9, Windows.UI.Colors.White, 3);
        }

    }
}
