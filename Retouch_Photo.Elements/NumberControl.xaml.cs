using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Retouch_Photo.Elements
{
    public sealed partial class NumberControl : UserControl
    {
        double value;

        public NumberControl()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ToNumber(this.TextBox.Text);
        }
        private void ToNumber(string text)
        {
            if (this.IsNumeric(text))
            {
                this.value = double.Parse(text);
                this.Slider.Value = this.value;
            }
        }


        public bool IsNumeric(string str)
        {
            return int.TryParse(str, out int _integer);
        }



        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.value = e.NewValue;
            this.TextBox.Text = this.value.ToString();
        }
    }
}
