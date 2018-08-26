using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pickers
{
    public sealed partial class HSLPicker : UserControl
    {

        //Delegate
        public delegate void ColorChangeHandler(object sender, Color value);
        public event ColorChangeHandler ColorChange = null;


        #region DependencyProperty


        private Color color = Color.FromArgb(255,255,255,255);
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
        public static readonly DependencyProperty HSLProperty = DependencyProperty.Register(nameof(HSL), typeof(HSL), typeof(HSLPicker), new PropertyMetadata(new HSL(255,360,100, 100), new PropertyChangedCallback(HSLOnChanged)));
        private static void HSLOnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            HSLPicker con = (HSLPicker)sender;

            if (e.NewValue is HSL NewValue) con.HSLChanged(NewValue);
        }
        private void HSLChanged(HSL value)
        {
            byte A = value.A;
            double H = value.H;
            double S = value.S;
            double L = value.L;

            //H          
           this. HSlider.Value = this.HPicker.Value = (int)H;
            this.HG.Color = this.HA.Color = HSL.HSLtoRGB(A, 0, S, L);
            this.HB.Color = HSL.HSLtoRGB(A, 60, S, L);
            this.HC.Color = HSL.HSLtoRGB(A, 120, S, L);
            this.HD.Color = HSL.HSLtoRGB(A, 180, S, L);
            this.HE.Color = HSL.HSLtoRGB(A, 240, S, L);
            this.HF.Color = HSL.HSLtoRGB(A, 300, S, L);
            //S
            this.SSlider.Value = SPicker.Value = (int)S;
            this.SLeft.Color = HSL.HSLtoRGB(A, H, 0.0d, L);
            this.SRight.Color = HSL.HSLtoRGB(A, H, 100.0d, L);
            //L
            this.LSlider.Value = LPicker.Value = (int)L;
            this.LLeft.Color = HSL.HSLtoRGB(A, H, S, 0.0d);
            this.LRight.Color = HSL.HSLtoRGB(A, H, S, 100.0d);

            this.color = HSL.HSLtoRGB(A, H, S, L);
            this.ColorChange?.Invoke(this, this.color);
        }


        #endregion


        public HSLPicker()
        {
            this.InitializeComponent();
        }


        private void HSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.HSL = new HSL(this.HSL.A, e.NewValue, this.HSL.S, this.HSL.L);
        private void SSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.HSL = new HSL(this.HSL.A, this.HSL.H, (int)e.NewValue, this.HSL.L);
        private void LSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.HSL = new HSL(this.HSL.A, this.HSL.H, this.HSL.S, (int)e.NewValue);

        private void HPicker_ValueChange(object sender, int Value) => this.HSL = new HSL(this.HSL.A, Value, this.HSL.S, this.HSL.L);
        private void SPicker_ValueChange(object sender, int Value) => this.HSL = new HSL(this.HSL.A, this.HSL.H, (int)Value, this.HSL.L);
        private void LPicker_ValueChange(object sender, int Value) => this.HSL = new HSL(this.HSL.A, this.HSL.H, this.HSL.S, (int)Value);

    }
}
