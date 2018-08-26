using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Retouch_Photo.Pickers
{
    public sealed partial class AlphaPicker : UserControl
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
            AlphaPicker con = (AlphaPicker)sender;

            if (e.NewValue is Color NewValue) con.ColorChanged(NewValue);
        }
        private void ColorChanged(Color value)
        {
            this.Slider.Value = value.A;
            this.Picker.Value = value.A;
            this.Left.Color = Color.FromArgb(0, value.R, value.G, value.B);
            this.Right.Color = Color.FromArgb(255, value.R, value.G, value.B);

            this.TextBox.Text = this.ColorToString(value);

            this.ColorChange?.Invoke(this, value);
        }


        #endregion


        public AlphaPicker()
        {
            this.InitializeComponent();
        }


        private void Picker_ValueChange(object sender, int value)=>  this.Color = Windows.UI.Color.FromArgb((byte)value, this.Color.R, this.Color.G, this.Color.B);
        private void Slider_ValueChangeDelta(object sender, RangeBaseValueChangedEventArgs e) => this.Color = Windows.UI.Color.FromArgb((byte)e.NewValue, this.Color.R, this.Color.G, this.Color.B);

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        private Color? StringToColor(string s)
        {
            if (s.Length == 6) return Color.FromArgb(255, (byte)int.Parse(s.Substring(0, 2), NumberStyles.HexNumber), (byte)int.Parse(s.Substring(2, 2), NumberStyles.HexNumber), (byte)int.Parse(s.Substring(4, 2), NumberStyles.HexNumber));
            else if (s.Length == 8) return Color.FromArgb((byte)int.Parse(s.Substring(0, 2), NumberStyles.HexNumber), (byte)int.Parse(s.Substring(2, 2), NumberStyles.HexNumber), (byte)int.Parse(s.Substring(4, 2), NumberStyles.HexNumber), (byte)int.Parse(s.Substring(6, 2), NumberStyles.HexNumber));
            return null;
        }
        private string ColorToString(Color c) => (c.R.ToString("x2") + c.G.ToString("x2") + c.B.ToString("x2").ToString()).ToUpper();

    }
}
