// Core:              
// Referenced:   ★★★
// Difficult:         ★
// Only:              ★
// Complete:      ★★
using Microsoft.Toolkit.Uwp.UI;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Width and height picker.
    /// </summary>
    public sealed partial class SizePicker : UserControl
    {

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
                this.WidthTextBox.Text = width.ToString();
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
                this.HeightTextBox.Text = height.ToString();
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
            this.WidthTextBox.LostFocus += (s, e) =>
            {
                if (this.WidthTextBox.Text is string value)
                {
                    if (string.IsNullOrEmpty(value)) return;

                    double width = double.Parse(value);

                    if (this.RatioToggleControl.IsChecked == false)
                    {
                        if (width < this.Minimum) this.WidthTextBox.Text = this.Minimum.ToString();
                        else if (width > this.Maximum) this.WidthTextBox.Text = this.Maximum.ToString();
                    }
                    else
                    {
                        double height = width / this.CacheWidth * this.SizeHeight;

                        this.CacheWidth = width;
                        this.SizeHeight = this.CacheHeight = height;
                    }
                }
            };

            this.HeightTextBox.Text = 1024.ToString();
            TextBoxExtensions.SetDefault(this.HeightTextBox, 1024.ToString());
            this.HeightTextBlock.LostFocus += (s, e) =>
            {
                if (this.HeightTextBlock.Text is string value)
                {
                    if (string.IsNullOrEmpty(value)) return;

                    double height = double.Parse(value);

                    if (this.RatioToggleControl.IsChecked == false)
                    {
                        if (height < this.Minimum) this.HeightTextBlock.Text = this.Minimum.ToString();
                        else if (height > this.Maximum) this.HeightTextBlock.Text = this.Maximum.ToString();
                    }
                    else
                    {
                        double width = height / this.CacheHeight * this.SizeWith;

                        this.CacheHeight = height;
                        this.SizeHeight = this.CacheHeight = width;
                    }
                }
            };
        }
    }
}