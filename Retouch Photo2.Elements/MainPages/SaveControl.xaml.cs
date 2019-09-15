using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary> Format of <see cref="SaveControl"/>. </summary>
    public enum SaveFormatType
    {
        JPEG,
        PNG,
        BMP,
        GIF,
        TIFF,
    }

    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "SaveControl" />.
    /// </summary>
    public sealed partial class SaveControl : UserControl
    {
        //@Content
        /// <summary> Format. </summary>
        public SaveFormatType Format { get => (SaveFormatType)this.ComboBox.SelectedIndex; set => this.ComboBox.SelectedIndex = (int)value; }
        /// <summary> <see cref = "SaveControl" />'s OKButton. </summary>
        public Button OKButton => this._OKButton.RootButton;
        /// <summary> <see cref = "SaveControl" />'s CancelButton. </summary>
        public Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public SaveControl()
        {
            this.InitializeComponent();
        }
    }
}