using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// Image save format ComboBox.
    /// </summary>
    public sealed partial class FormatComboBox : UserControl
    {
        //@Content
        /// <summary> Format. </summary>
        public FormatType Format { get => (FormatType)this.ComboBox.SelectedIndex; set => this.ComboBox.SelectedIndex = (int)value; }
     
        /// <summary> File Format. </summary>
        public CanvasBitmapFileFormat FileFormat
        {
            get
            {
                switch (this.Format)
                {
                    case FormatType.JPEG: return CanvasBitmapFileFormat.Jpeg;
                    case FormatType.PNG: return CanvasBitmapFileFormat.Png;
                    case FormatType.BMP: return CanvasBitmapFileFormat.Bmp;
                    case FormatType.GIF: return CanvasBitmapFileFormat.Gif;
                    case FormatType.TIFF: return CanvasBitmapFileFormat.Tiff;
                    default: return CanvasBitmapFileFormat.Jpeg;
                }
            }
        }
        /// <summary> File Choices. </summary>
        public string FileChoices
        {
            get
            {
                switch (this.Format)
                {
                    case FormatType.JPEG: return ".Jpeg";
                    case FormatType.PNG: return ".Png";
                    case FormatType.BMP: return ".Bmp";
                    case FormatType.GIF: return ".Gif";
                    case FormatType.TIFF: return ".Tiff";
                    default: return ".Jpeg";
                }
            }
        }


        //@Construct
        public FormatComboBox()
        {
            this.InitializeComponent();
        }
    }
}