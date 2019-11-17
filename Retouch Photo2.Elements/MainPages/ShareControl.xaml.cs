using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "ShareControl" />.
    /// </summary>
    public sealed partial class ShareControl : UserControl
    {
        //@Content
        /// <summary> Format ComboBox. </summary>
        public FormatComboBox ComboBox => this._ComboBox;
        /// <summary> <see cref = "ShareControl" />'s OKButton. </summary>
        public Button OKButton => this._OKButton.RootButton;
        /// <summary> <see cref = "ShareControl" />'s CancelButton. </summary>
        public Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public ShareControl()
        {
            this.InitializeComponent();
        }
    }
}