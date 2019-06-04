using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Pages.MainPages
{
    /// <summary> Format of <see cref="ShareControl"/>. </summary>
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

        /// <summary> Format. </summary>
        public SaveFormatType Format
        {
            get => (SaveFormatType)this.ComboBox.SelectedIndex;
            set => this.ComboBox.SelectedIndex = (int)value;
        }

        /// <summary> <see cref = "ShareControl" />'s OKButton. </summary>
        public Windows.UI.Xaml.Controls.Button OKButton { get => this._OKButton.RootButton; set => this._OKButton.RootButton = value; }
        /// <summary> <see cref = "ShareControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton { get => this._CancelButton.RootButton; set => this._CancelButton.RootButton = value; }

        public ShareControl()
        {
            this.InitializeComponent();
        }
    }
}
