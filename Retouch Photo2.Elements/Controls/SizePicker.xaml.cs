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
        public BitmapSize Size => new BitmapSize()
        {
            Width = (uint)this.WidthNumberPicker.Value,
            Height = (uint)this.HeighNumberPicker.Value
        };

        //@Construct
        public SizePicker()
        {
            this.InitializeComponent();

            this.WidthNumberPicker.Unit = "px";
            this.HeighNumberPicker.Unit = "px";
            this.WidthNumberPicker.Minimum = 16;
            this.HeighNumberPicker.Minimum = 16;
            this.WidthNumberPicker.Maximum = 16384;
            this.HeighNumberPicker.Maximum = 16384;
            this.WidthNumberPicker.Value = 1024;
            this.HeighNumberPicker.Value = 1024;
        }
    }
}