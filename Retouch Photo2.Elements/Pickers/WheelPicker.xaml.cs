using HSVColorPickers;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Pick a color in the wheel.
    /// </summary>
    public sealed partial class WheelPicker : UserControl, IColorPicker, IHSVPicker
    {

        #region Helpher


        private sealed class WheelSize
        {
            public static float VectorToH(Vector2 vector) => ((((float)Math.Atan2(vector.Y, vector.X)) * 180.0f / (float)Math.PI) + 360.0f) % 360.0f;
            public static float VectorToS(float vectorX, float squareRadio) => vectorX * 50 / squareRadio + 50;
            public static float VectorToV(float vectorY, float squareRadio) => 50 - vectorY * 50 / squareRadio;

            public static Vector2 HToVector(float h, float radio, Vector2 center) => new Vector2((float)Math.Cos(h) * radio + center.X, (float)Math.Sin(h) * radio + center.Y);
            public static float SToVector(float s, float squareRadio, float centerX) => ((float)s - 50) * squareRadio / 50 + centerX;
            public static float VToVector(float v, float squareRadio, float centerY) => (50 - (float)v) * squareRadio / 50 + centerY;
        }


        #endregion


        //@Delegate
        /// <summary> Occurs when the color value changes. </summary>
        public event ColorChangeHandler ColorChange = null;
        /// <summary> Occurs when the hsv value changes. </summary>
        public event HSVChangeHandler HSVChange = null;


        /// <summary> Gets picker's type name. </summary>
        public string Type => "Wheel";
        /// <summary> Gets picker self. </summary>
        public UserControl Self => this;

        /// <summary> Gets or sets picker's color. </summary>
        public Color Color
        {
            get => HSV.HSVtoRGB(this.HSV);
            set => this.HSV = HSV.RGBtoHSV(value);
        }


        #region Color


        /// <summary> Gets or sets picker's hsv. </summary>
        public HSV HSV
        {
            get => this.hsv;
            set
            {
                //Palette                
                this.HorizontalColor.Color = HSV.HSVtoRGB(value.H);
                this.UpdateThumb(value);

                this.hsv = value;
            }
        }

        private HSV hsv = new HSV(255, 0, 100, 100);
        private HSV _HSV
        {
            get => this.hsv;
            set
            {
                this.ColorChange?.Invoke(this, HSV.HSVtoRGB(value));//Delegate
                this.HSVChange?.Invoke(this, value);//Delegate

                //Palette  
                this.HorizontalColor.Color = HSV.HSVtoRGB(value.H);
                this.UpdateThumb(value);

                this.hsv = value;
            }
        }


        #endregion


        readonly float _strokeWidth = 8;
        float _canvasWidth;
        float _canvasHeight;

        //Wheel
        Vector2 _center;// Wheel's center
        float _radio;// Wheel's radio
        float _radioSpace;

        //Palette  
        float _square;
        Rect _rect;

        //Manipulation
        bool _isWheel;
        bool _isPalette;
        Vector2 _position;


        //@Construct
        /// <summary>
        /// Construct a WheelPicker.
        /// </summary>
        public WheelPicker()
        {
            this.InitializeComponent();

            //Canvas
            this.Canvas.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this._canvasWidth = width;
                this._canvasHeight = height;

                //Wheel
                this._center.X = width / 2;
                this._center.Y = height / 2;
                this._radio = Math.Min(width, height) / 2 - this._strokeWidth;
                this._radio = Math.Max(this._radio, 20);
                this._radioSpace = (float)(2 * Math.PI) / (int)(Math.PI * this._radio * 2 / this._strokeWidth);

                // Palette          
                this._square = (this._radio - this._strokeWidth) / 1.414213562373095f;
                this._square = Math.Max(this._square, 20);
                this._rect = new Rect(this._center.X - this._square, this._center.Y - this._square, this._square * 2, this._square * 2);

                this.UpdateEllipse(this._radio, this._strokeWidth);
                this.UpdateRectangle(this._rect);
                this.UpdateThumb(this.hsv);
            };
            this.Canvas.Loaded += (s, e) =>
            {
                // Palette       
                this.HorizontalColor.Color = HSV.HSVtoRGB(this.hsv.H);
                this.UpdateRectangle(this._rect);
                this.UpdateThumb(this.hsv);
            };


            //Manipulation
            this.Canvas.ManipulationMode = ManipulationModes.All;
            this.Canvas.ManipulationStarted += (s, e) =>
            {
                this._position = e.Position.ToVector2() - this._center;

                this._isWheel = this._position.Length() + this._strokeWidth > this._radio && this._position.Length() - this._strokeWidth < this._radio;
                this._isPalette = Math.Abs(this._position.X) < this._square && Math.Abs(this._position.Y) < this._square;

                if (this._isWheel) this._HSV = new HSV(this.hsv.A, WheelSize.VectorToH(this._position), this.hsv.S, this.hsv.V);
                if (this._isPalette) this._HSV = new HSV(this.hsv.A, this.hsv.H, WheelSize.VectorToS(this._position.X, this._square), WheelSize.VectorToV(this._position.Y, this._square));
            };
            this.Canvas.ManipulationDelta += (s, e) =>
            {
                this._position += e.Delta.Translation.ToVector2();

                if (this._isWheel) this._HSV = new HSV(this.hsv.A, WheelSize.VectorToH(this._position), this.hsv.S, this.hsv.V);
                if (this._isPalette) this._HSV = new HSV(this.hsv.A, this.hsv.H, WheelSize.VectorToS(this._position.X, this._square), WheelSize.VectorToV(this._position.Y, this._square));
            };
            this.Canvas.ManipulationCompleted += (s, e) =>
            {
                this._isWheel = false;
                this._isPalette = false;
            };
        }


        private void UpdateThumb(HSV hSV)
        {
            //Thumb
            Vector2 wheel = WheelSize.HToVector((float)((hSV.H + 360.0) * Math.PI / 180.0), this._radio, this._center);
            Canvas.SetLeft(this.WheelThumb1, wheel.X - 9);
            Canvas.SetTop(this.WheelThumb1, wheel.Y - 9);

            Canvas.SetLeft(this.WheelThumb2, wheel.X - 8);
            Canvas.SetTop(this.WheelThumb2, wheel.Y - 8);


            //Thumb 
            float paletteX = WheelSize.SToVector(hSV.S, this._square, this._center.X);
            float paletteY = WheelSize.VToVector(hSV.V, this._square, this._center.Y);
            Canvas.SetLeft(this.PaletteThumb1, paletteX - 9);
            Canvas.SetTop(this.PaletteThumb1, paletteY - 9);

            Canvas.SetLeft(this.PaletteThumb2, paletteX - 8);
            Canvas.SetTop(this.PaletteThumb2, paletteY - 8);
        }

        private void UpdateEllipse(float radio, float strokeWidth)
        {
            double wheel = this._radio + this._strokeWidth;
            Canvas.SetLeft(this.WheelEllipse, this._center.X - wheel);
            Canvas.SetTop(this.WheelEllipse, this._center.Y - wheel);
            this.WheelEllipse.Width =
            this.WheelEllipse.Height = wheel * 2;

            double Hole = this._radio - this._strokeWidth;
            Canvas.SetLeft(this.HoleEllipse, this._center.X - Hole);
            Canvas.SetTop(this.HoleEllipse, this._center.Y - Hole);
            this.HoleEllipse.Width =
            this.HoleEllipse.Height = Hole * 2;
        }

        private void UpdateRectangle(Rect rect)
        {
            Canvas.SetLeft(this.HorizontalRectangle, rect.X);
            Canvas.SetTop(this.HorizontalRectangle, rect.Y);
            this.HorizontalRectangle.Width = rect.Width;
            this.HorizontalRectangle.Height = rect.Height;

            Canvas.SetLeft(this.VerticalRectangle, rect.X);
            Canvas.SetTop(this.VerticalRectangle, rect.Y);
            this.VerticalRectangle.Width = rect.Width;
            this.VerticalRectangle.Height = rect.Height;
        }

    }
}