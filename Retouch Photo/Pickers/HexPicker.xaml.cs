using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo.Pickers
{
    public class Hex
    {
        /// <summary> Hex Number To Color </summary>
        public static Color IntToColor(int hexNumber) => Color.FromArgb(255, (byte)((hexNumber >> 16) & 0xff), (byte)((hexNumber >> 8) & 0xff), (byte)((hexNumber >> 0) & 0xff));

        /// <summary> String To Hex Number </summary>
        public static int StringToInt(string hex) => int.Parse(hex, System.Globalization.NumberStyles.HexNumber);

        /// <summary> String To Color </summary>
        public static string ColorToString(Color color) => color.R.ToString("x2") + color.G.ToString("x2") + color.B.ToString("x2").ToString();

        /// <summary> Subste </summary>
        public static string TextSubstring(string text)
        {
            if (text == null) return null;

            if (text.Length < 6) return null;

            if (text.Length == 6) return text;

            return text.Substring(text.Length - 6, 6);
        }
    }

    public sealed partial class HexPicker : UserControl
    {

        //Delegate
        public event ColorChangeHandler ColorChange = null;

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
        public Color Color
        {
            get => this.color;
            set
            {
                this.TextBox.Text = Hex.ColorToString(value).ToUpper();
                this.color = value;
            }
        }


        #endregion

        public HexPicker()
        {
            this.InitializeComponent();

            this.TextBox.GotFocus += (object sender, RoutedEventArgs e) => { };
            this.TextBox.LostFocus += (object sender, RoutedEventArgs e) => this.Color = this._Color = this.TextHex(this.TextBox.Text);
        }

        private Color TextHex(string text)
        {
            string hex = Hex.TextSubstring(text);

            if (hex == null) return this.color;

            try
            {
                return Hex.IntToColor(Hex.StringToInt(hex));
            }
            catch (Exception)
            {
                return this.color;
            }
        }
    }
}
