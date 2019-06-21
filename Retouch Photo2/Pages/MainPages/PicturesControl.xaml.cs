using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "PicturesControl" />.
    /// </summary>
    public sealed partial class PicturesControl : UserControl
    {

        /// <summary> <see cref = "PicturesControl" />'s PhotoButton. </summary>
        public Windows.UI.Xaml.Controls.Button PhotoButton { get => this._PhotoButton.RootButton; set => this._PhotoButton.RootButton = value; }
        /// <summary> <see cref = "PicturesControl" />'s DestopButton. </summary>
        public Windows.UI.Xaml.Controls.Button DestopButton { get => this._DestopButton.RootButton; set => this._DestopButton.RootButton = value; }

        /// <summary> <see cref = "PicturesControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton { get => this._CancelButton.RootButton; set => this._CancelButton.RootButton = value; }

        public PicturesControl()
        {
            this.InitializeComponent();
        }
    }
}
