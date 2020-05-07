using Microsoft.Graphics.Canvas;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    /// <summary>
    /// ComboBox of <see cref="CanvasBitmapFileFormat"/>.
    /// </summary>
    public sealed partial class FileFormatComboBox : UserControl
    {

        //@VisualState
        CanvasBitmapFileFormat _vsFileFormat;
        public VisualState VisualState
        {
            get
            {
                switch (this._vsFileFormat)
                {
                    case CanvasBitmapFileFormat.Jpeg: return this.Jpeg;
                    case CanvasBitmapFileFormat.Png: return this.Png;
                    case CanvasBitmapFileFormat.Bmp: return this.Bmp;
                    case CanvasBitmapFileFormat.Gif: return this.Gif;
                    case CanvasBitmapFileFormat.Tiff: return this.Tiff;
                    default: return this.Normal;
                }
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets the format type. </summary>
        public CanvasBitmapFileFormat FileFormat
        {
            get { return (CanvasBitmapFileFormat)GetValue(FileFormatProperty); }
            set { SetValue(FileFormatProperty, value); }
        }
        /// <summary> Identifies the <see cref = "FileFormatComboBox.FileFormat" /> dependency property. </summary>
        public static readonly DependencyProperty FileFormatProperty = DependencyProperty.Register(nameof(FileFormat), typeof(CanvasBitmapFileFormat), typeof(FileFormatComboBox), new PropertyMetadata(CanvasBitmapFileFormat.Jpeg, (sender, e) =>
        {
            FileFormatComboBox con = (FileFormatComboBox)sender;

            if (e.NewValue is CanvasBitmapFileFormat value)
            {
                con._vsFileFormat = value;
                con.VisualState = con.VisualState;//State
            }
        }));


        #endregion


        //@Construct
        public FileFormatComboBox()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => this.VisualState = this.VisualState;//State
            this.JpegButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Jpeg;
            this.PngButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Png;
            this.BmpButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Bmp;
            this.GifButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Gif;
            this.TiffButton.Tapped += (s, e) => this.FileFormat = CanvasBitmapFileFormat.Tiff;
        }
        

        /// <summary> File Choices. </summary>
        public string FileChoices
        {
            get
            {
                switch (this.FileFormat)
                {
                    case CanvasBitmapFileFormat.Jpeg: return ".Jpeg";
                    case CanvasBitmapFileFormat.Png: return ".Png";
                    case CanvasBitmapFileFormat.Bmp: return ".Bmp";
                    case CanvasBitmapFileFormat.Gif: return ".Gif";
                    case CanvasBitmapFileFormat.Tiff: return ".Tiff";
                    default: return ".Jpeg";
                }
            }
        }

    }
}