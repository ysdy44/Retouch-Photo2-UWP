using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Extensions;


namespace Retouch_Photo.Pages.PageControls
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
            if (text.IsNumeric())
            {
                this.value = double.Parse(text);
                this.Slider.Value = this.value;
            }
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            this.value = e.NewValue;
            this.TextBox.Text = this.value.ToString();
        }
    }
}
