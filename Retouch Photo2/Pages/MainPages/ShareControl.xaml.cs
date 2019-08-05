using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.MainPages
{
    /// <summary>
    /// Format of <see cref="ShareControl"/>. 
    /// </summary>
    public enum ShareFormatType
    {
        JPEG,
        PNG,
        BMP,
        GIF,
        TIFF,
    }

    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "ShareControl" />.
    /// </summary>
    public sealed partial class ShareControl : UserControl
    {
        //@Content
        /// <summary> Format. </summary>
        public SaveFormatType Format { get => (SaveFormatType)this.ComboBox.SelectedIndex; set => this.ComboBox.SelectedIndex = (int)value; }
        /// <summary> <see cref = "ShareControl" />'s OKButton. </summary>
        public Windows.UI.Xaml.Controls.Button OKButton => this._OKButton.RootButton;
        /// <summary> <see cref = "ShareControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public ShareControl()
        {
            this.InitializeComponent();
        }
    }
}