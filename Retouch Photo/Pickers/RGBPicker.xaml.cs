using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace Retouch_Photo.Pickers
{
    public sealed partial class RGBPicker : UserControl
    {

        //Delegate
        public delegate void ColorChangeHandler(object sender, Color Value);
        public event ColorChangeHandler ColorChange = null;

        #region DependencyProperty


        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(HSLPicker), new PropertyMetadata(null, new PropertyChangedCallback(ColorOnChanged)));
        private static void ColorOnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RGBPicker con = (RGBPicker)sender;

            if (e.NewValue is Color NewValue) con.ColorChanged(NewValue);
        }
        private void ColorChanged(Color value)
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

            this.ColorChange?.Invoke(this,value);
        }


        #endregion


        public RGBPicker()
        {
            this.InitializeComponent();
        }


        //Slider
        private void RSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.Color = Color.FromArgb(this.Color.A, (byte)e.NewValue, this.Color.G, this.Color.B);
        private void GSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.Color = Color.FromArgb(this.Color.A, this.Color.R, (byte)e.NewValue, this.Color.B);
        private void BSlider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.Color = Color.FromArgb(this.Color.A, this.Color.R, this.Color.G, (byte)e.NewValue);

        //Picker
        private void RPicker_ValueChange(object sender, int Value) => this.Color = Color.FromArgb(this.Color.A, (byte)Value, this.Color.G, this.Color.B);
        private void GPicker_ValueChange(object sender, int Value) => this.Color = Color.FromArgb(this.Color.A, this.Color.R, (byte)Value, this.Color.B);
        private void BPicker_ValueChange(object sender, int Value) => this.Color = Color.FromArgb(this.Color.A, this.Color.R, this.Color.G, (byte)Value);
       
    }
}
