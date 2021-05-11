// Core:              
// Referenced:   ★★★
// Difficult:         ★
// Only:              ★
// Complete:      ★★
using Microsoft.Toolkit.Uwp.UI;
using System;
using Windows.Graphics.Imaging;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Width and height picker.
    /// </summary>
    public sealed partial class SizePicker : UserControl
    {

        //@Converter
        private string Round2Converter(double value) => $"{Math.Round(value, 2)}";

        //@Content
        /// <summary> WidthTextBlock's Text. </summary>
        public string WidthText { get => this.WidthTextBlock.Text; set => this.WidthTextBlock.Text = value; }
        /// <summary> HeightTextBlock's Text. </summary>
        public string HeightText { get => this.HeightTextBlock.Text; set => this.HeightTextBlock.Text = value; }

        /// <summary> Minimum. Defult 16. </summary>
        public int Minimum { get; set; } = 16;
        /// <summary> Maximum. Defult 16384. </summary>
        public int Maximum { get; set; } = 16384;

        /// <summary> Size. </summary>
        public BitmapSize Size => new BitmapSize
        {
            Width = (uint)this.SizeWith,
            Height = (uint)this.SizeHeight,
        };
        /// <summary> Width for size. </summary>
        public double SizeWith
        {
            get => (uint)double.Parse(this.WidthTextBox.Text);
            set
            {
                double width = value;
                if (width < this.Minimum) width = this.Minimum;
                else if (width > this.Maximum) width = this.Maximum;

                this.WidthTextBox.Text = this.Round2Converter(width);
            }
        }
        /// <summary> Height for size. </summary>
        public double SizeHeight
        {
            get => (uint)double.Parse(this.HeightTextBox.Text);
            set
            {
                double height = value;
                if (height < this.Minimum) height = this.Minimum;
                else if (height > this.Maximum) height = this.Maximum;

                this.HeightTextBox.Text = this.Round2Converter(height);
            }
        }

        private double CacheWidth = 1024;
        private double CacheHeight = 1024;

        //@Construct
        /// <summary>
        /// Initializes a SizePicker. 
        /// </summary>
        public SizePicker()
        {
            this.InitializeComponent();

            this.WidthTextBox.Text = 1024.ToString();
            TextBoxExtensions.SetDefault(this.WidthTextBox, 1024.ToString());
            this.WidthTextBox.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.Focus(FocusState.Programmatic); };
            this.WidthTextBox.LostFocus += (s, e) =>
            {
                if (this.WidthTextBox.Text is string value)
                {
                    if (string.IsNullOrEmpty(value)) return;

                    double width = double.Parse(value);

                    if (width < this.Minimum)
                    {
                        width = this.Minimum;
                        this.WidthTextBox.Text = this.Minimum.ToString();
                    }
                    else if (width > this.Maximum)
                    {
                        width = this.Maximum;
                        this.WidthTextBox.Text = this.Maximum.ToString();
                    }

                    if (this.RatioToggleControl.IsChecked == true)
                    {
                        double height = width / this.CacheWidth * this.SizeHeight;
                        this.CacheHeight = height;
                        this.SizeHeight = height;
                    }
                    this.CacheWidth = width;
                }
            };

            this.HeightTextBox.Text = 1024.ToString();
            TextBoxExtensions.SetDefault(this.HeightTextBox, 1024.ToString());
            this.HeightTextBox.KeyDown += (s, e) => { if (e.Key == VirtualKey.Enter) this.Focus(FocusState.Programmatic); };
            this.HeightTextBox.LostFocus += (s, e) =>
            {
                if (this.HeightTextBox.Text is string value)
                {
                    if (string.IsNullOrEmpty(value)) return;

                    double height = double.Parse(value);

                    if (height < this.Minimum)
                    {
                        height = this.Minimum;
                        this.HeightTextBox.Text = this.Minimum.ToString();
                    }
                    else if (height > this.Maximum)
                    {
                        height = this.Maximum;
                        this.HeightTextBox.Text = this.Maximum.ToString();
                    }

                    if (this.RatioToggleControl.IsChecked == true)
                    {
                        double width = height / this.CacheHeight * this.SizeWith;
                        this.CacheWidth = width;
                        this.SizeWith = width;
                    }
                    this.CacheHeight = height;
                }
            };
        }
    }
}