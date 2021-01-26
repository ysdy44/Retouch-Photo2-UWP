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
        /// <summary> Size. </summary>
        public BitmapSize Size
        {
            get => new BitmapSize()
            {
                Width = (uint)this.WidthPicker.Value,
                Height = (uint)this.HeighPicker.Value
            };
            set
            {
                this.WidthPicker.Value = (int)value.Width;
                this.HeighPicker.Value = (int)value.Height;
            }
        }

        int cacheWidth = 1024;
        int cacheHeight = 1024;

        //@Construct
        /// <summary>
        /// Initializes a SizePicker. 
        /// </summary>
        public SizePicker()
        {
            this.InitializeComponent();

            this.WidthPicker.Unit = "px";
            this.HeighPicker.Unit = "px";
            this.WidthPicker.Minimum = 16;
            this.HeighPicker.Minimum = 16;
            this.WidthPicker.Maximum = 16384;
            this.HeighPicker.Maximum = 16384;

            this.WidthPicker.Value = 1024;
            this.HeighPicker.Value = 1024;
            this.WidthPicker.ValueChanged += (s, value) =>
            {
                if (this.RatioToggleControl.IsChecked == false) return;

                double width = value;
                double height = value / (double)this.cacheWidth * this.HeighPicker.Value;

                this.cacheWidth = (int)width;
                this.HeighPicker.Value = this.cacheHeight = (int)height;
            };

            this.HeighPicker.ValueChanged += (s, value) =>
            {
                if (this.RatioToggleControl.IsChecked == false) return;

                double width = value / (double)this.cacheHeight * this.WidthPicker.Value;
                double height = value;

                this.WidthPicker.Value = this.cacheWidth = (int)width;
                this.cacheHeight = (int)height;
            };

        }
    }
}