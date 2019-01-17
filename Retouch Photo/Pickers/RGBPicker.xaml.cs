using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace Retouch_Photo.Pickers
{
    public sealed partial class RGBPicker : UserControl, IPicker
    {

        //Delegate
        public event ColorChangeHandler ColorChange = null;
        public Color GetColor() => this.Color;
        public void SetColor(Color value) => this.Color = value;


        #region DependencyProperty


        private Color color = Color.FromArgb(255, 255, 255, 255);
        private Color _Color
        {
            get => this.color;
            set
            {
                this.ColorChange?.Invoke(this, value);

                this.color = value;
            }
        }
        private Color Color
        {
            get => this.color;
            set
            {
                //R
                this.RSlider.Value = value.R;
                this.RPicker.Value = value.R;
                this.RLeft.Color = Color.FromArgb(255, 0, value.G, value.B);
                this.RRight.Color = Color.FromArgb(255, 255, value.G, value.B);

                //G
                this.GSlider.Value = value.G;
                this.GPicker.Value = value.G;
                this.GLeft.Color = Color.FromArgb(255, value.R, 0, value.B);
                this.GRight.Color = Color.FromArgb(255, value.R, 255, value.B);

                //B
                this.BSlider.Value = value.B;
                this.BPicker.Value = value.B;
                this.BLeft.Color = Color.FromArgb(255, value.R, value.G, 0);
                this.BRight.Color = Color.FromArgb(255, value.R, value.G, 255);

                this.color = value;
            }
        }


        #endregion


        public RGBPicker()
        {
            this.InitializeComponent();
        }


        //Slider
        private void RSlider_ValueChange(object sender, double value) => this.Color = this._Color = Color.FromArgb(this.color.A, (byte)value, this.color.G, this.color.B);
        private void GSlider_ValueChange(object sender, double value) => this.Color = this._Color = Color.FromArgb(this.color.A, this.color.R, (byte)value, this.color.B);
        private void BSlider_ValueChange(object sender, double value) => this.Color = this._Color = Color.FromArgb(this.color.A, this.color.R, this.color.G, (byte)value);

        //Picker
        private void RPicker_ValueChange(object sender, int Value) => this.Color = this._Color = Color.FromArgb(this.color.A, (byte)Value, this.color.G, this.color.B);
        private void GPicker_ValueChange(object sender, int Value) => this.Color = this._Color = Color.FromArgb(this.color.A, this.color.R, (byte)Value, this.color.B);
        private void BPicker_ValueChange(object sender, int Value) => this.Color = this._Color = Color.FromArgb(this.color.A, this.color.R, this.color.G, (byte)Value);

    }
}
