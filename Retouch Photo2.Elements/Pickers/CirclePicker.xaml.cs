using System;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using HSVColorPickers;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Pick a color in the circle hue wheel.
    /// </summary>
    public sealed partial class CirclePicker : UserControl, IColorPicker, IHSVPicker
    {

        #region Helpher


        private sealed class CircleSize
        {
            public static float VectorToH(Vector2 vector) => ((((float)Math.Atan2(vector.Y, vector.X)) * 180.0f / (float)Math.PI) + 360.0f) % 360.0f;
            public static Vector2 HSToVector(float h, float s, float radio, Vector2 center) => new Vector2((float)Math.Cos(h), (float)Math.Sin(h)) * radio * s / 100.0f + center;
            public static float VectorToS(Vector2 vector, float radio)
            {
                float s = vector.Length() / radio;
                if (s < 0) return 0.0f;
                if (s > 1) return 100.0f;
                return s * 100.0f;
            }
        }


        #endregion


        //@Delegate
        /// <summary> Occurs when the color value changed. </summary>
        public event ColorChangeHandler ColorChanged;
        /// <summary> Occurs when the color change starts. </summary>
        public event ColorChangeHandler ColorChangeStarted;
        /// <summary> Occurs when color change. </summary>
        public event ColorChangeHandler ColorChangeDelta;
        /// <summary> Occurs when the color change is complete. </summary>
        public event ColorChangeHandler ColorChangeCompleted;
        /// <summary> Occurs when the hsv value changed. </summary>
        public event HSVChangeHandler HSVChanged = null;
        /// <summary> Occurs when the color change starts. </summary>
        public event HSVChangeHandler HSVChangeStarted;
        /// <summary> Occurs when color change. </summary>
        public event HSVChangeHandler HSVChangeDelta;
        /// <summary> Occurs when the color change is complete. </summary>
        public event HSVChangeHandler HSVChangeCompleted;


        /// <summary> Gets picker's type name. </summary>
        public string Type => "Circle";
        /// <summary> Gets picker self. </summary>
        public Control Self => this;

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
                byte A = value.A;
                float H = value.H;
                float S = value.S;
                float V = value.V;


                this.UpdateStop(H);
                this.UpdateSlider(V);

                this.UpdateColor(HSV.HSVtoRGB(value));
                this.UpdateThumb(H, S);

                this.hsv = value;
            }
        }

        private HSV hsv = new HSV(255, 0, 100, 100);
        private HSV _HSV
        {
            get => this.hsv;
            set
            {
                this.ColorChanged?.Invoke(this, HSV.HSVtoRGB(value));//Delegate
                this.HSVChanged?.Invoke(this, value);//Delegate

                this.hsv = value;
            }
        }
        private HSV _HSVStarted
        {
            get => this.hsv;
            set
            {
                this.ColorChangeStarted?.Invoke(this, HSV.HSVtoRGB(value));//Delegate
                this.HSVChangeStarted?.Invoke(this, value);//Delegate

                this.hsv = value;
            }
        }
        private HSV _HSVDelta
        {
            get => this.hsv;
            set
            {
                this.ColorChangeDelta?.Invoke(this, HSV.HSVtoRGB(value));//Delegate
                this.HSVChangeDelta?.Invoke(this, value);//Delegate

                this.hsv = value;
            }
        }
        private HSV _HSVCompleted
        {
            get => this.hsv;
            set
            {
                this.ColorChangeCompleted?.Invoke(this, HSV.HSVtoRGB(value));//Delegate
                this.HSVChangeCompleted?.Invoke(this, value);//Delegate

                this.hsv = value;
            }
        }


        #endregion


        float _canvasWidth;
        float _canvasHeight;

        //Circle
        Vector2 _center;// Circle's center
        float _radio;// Circle's radio
        float _maxRadio;

        //Manipulation
        Vector2 _position;


        //@Construct
        /// <summary>
        /// Initializes a CirclePicker.
        /// </summary>
        public CirclePicker()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this._canvasWidth = width;
                this._canvasHeight = height;
                this._center = new Vector2(width, height) / 2;

                float radio = Math.Min(width, height) / 2;
                this._radio = radio;

                int size = (int)(radio * 2);

                this.UpdateEllipse(size);
                this.UpdateSlider(this.hsv.V);
                this.Change(this.hsv);
            };

            //Image
            this.RootGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                float width = (float)e.NewSize.Width;
                float height = (float)e.NewSize.Height;

                this._canvasWidth = width;
                this._canvasHeight = height;
                this._center = new Vector2(width, height) / 2;

                float radio = Math.Min(width, height) / 2;
                this._radio = radio;

                int size = (int)(radio * 2);

                if (this._maxRadio + 30 < radio)//30 cache width, for performance optimization.
                {
                    this._maxRadio = radio;
                }

                this.UpdateEllipse(size);
                this.UpdateThumb(this.hsv.H, this.hsv.S);
            };

            //Manipulation
            this.Canvas.ManipulationMode = ManipulationModes.All;
            this.Canvas.ManipulationStarted += (s, e) =>
            {
                this._position = e.Position.ToVector2() - this._center;
                this._HSVStarted = this.Change(this._position);
            };
            this.Canvas.ManipulationDelta += (s, e) =>
            {
                this._position += e.Delta.Translation.ToVector2();
                this._HSVDelta = this.Change(this._position);
            };
            this.Canvas.ManipulationCompleted += (s, e) =>
            {
                this._HSVCompleted = this.Change(this._position);
            };

            //Slider
            this.VSlider.ValueChangeStarted += (sender, value) => this._HSVStarted = this.Change((float)value);
            this.VSlider.ValueChangeDelta += (sender, value) => this._HSVDelta = this.Change((float)value);
            this.VSlider.ValueChangeCompleted += (sender, value) => this._HSVCompleted = this.Change((float)value);

        }


        #region Change


        private HSV Change(Vector2 position)
        {
            float H = CircleSize.VectorToH(position);
            float S = CircleSize.VectorToS(position, this._radio);
            HSV hsv = new HSV(255, H, S, this.hsv.V);

            this.Change(hsv);
            return hsv;
        }
        private HSV Change(float value)
        {
            float V = value;
            HSV hsv = new HSV(255, this.hsv.H, this.hsv.S, V);

            this.Change(hsv);
            return hsv;
        }
        private void Change(HSV hsv)
        {
            this.UpdateStop(hsv.H);

            this.UpdateColor(HSV.HSVtoRGB(hsv));
            this.UpdateThumb(hsv.H, hsv.S);
        }


        #endregion

        #region Update


        private void UpdateStop(float H) => this.VRight.Color = HSV.HSVtoRGB(H);
        private void UpdateSlider(float V) => this.VSlider.Value = this.hsv.V;

        private void UpdateColor(Color color) => this.SolidColorBrush.Color = color;
        private void UpdateThumb(float H, float S)
        {
            Vector2 wheel = CircleSize.HSToVector((float)((H + 360.0) * Math.PI / 180.0), S, this._radio, this._center);
            Thumb thumb = this.HSThumb;
            Canvas.SetLeft(thumb, wheel.X - thumb.ActualWidth / 2);
            Canvas.SetTop(thumb, wheel.Y - thumb.ActualHeight / 2);
        }

        private void UpdateEllipse(int size) => this.HSEllipse.Width = this.HSEllipse.Height = size;


        #endregion

    }
}