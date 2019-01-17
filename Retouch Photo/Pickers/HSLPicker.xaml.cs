using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Pickers
{
    public sealed partial class HSLPicker : UserControl, IPicker
    {

        //Delegate
        public event ColorChangeHandler ColorChange = null;
        public Color GetColor() => HSL.HSLtoRGB(this.HSL);
        public void SetColor(Color value) => this.HSL = HSL.RGBtoHSL(value);


        #region DependencyProperty


        private HSL hsl = new HSL { A = 255, H = 0, S = 1, L = 1 };
        private HSL _HSL
        {
            get => this.hsl;
            set
            {
                this.ColorChange?.Invoke(this, HSL.HSLtoRGB(value.A, value.H, value.S, value.L));

                this.hsl = value;
            }
        }
        public HSL HSL
        {
            get => this.hsl;
            set
            {
                byte A = value.A;
                double H = value.H;
                double S = value.S;
                double L = value.L;

                //H          
                this.HSlider.Value = this.HPicker.Value = (int)H;
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

                this.hsl = value;
            }
        }


        #endregion


        public HSLPicker()
        {
            this.InitializeComponent();
        }


        private void HSlider_ValueChange(object sender, double value) => this.HSL = this._HSL = new HSL(this.hsl.A, value, this.hsl.S, this.hsl.L);
        private void SSlider_ValueChange(object sender, double value) => this.HSL = this._HSL = new HSL(this.hsl.A, this.hsl.H, value, this.hsl.L);
        private void LSlider_ValueChange(object sender, double value) => this.HSL = this._HSL = new HSL(this.hsl.A, this.hsl.H, this.hsl.S, value);

        private void HPicker_ValueChange(object sender, int Value) => this.HSL = this._HSL = new HSL(this.hsl.A, Value, this.hsl.S, this.HSL.L);
        private void SPicker_ValueChange(object sender, int Value) => this.HSL = this._HSL = new HSL(this.hsl.A, this.hsl.H, Value, this.hsl.L);
        private void LPicker_ValueChange(object sender, int Value) => this.HSL = this._HSL = new HSL(this.hsl.A, this.hsl.H, this.hsl.S, Value);

    }
}
