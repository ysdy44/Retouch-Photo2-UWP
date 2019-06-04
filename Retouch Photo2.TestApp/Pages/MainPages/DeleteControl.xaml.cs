using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.TestApp.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "DeleteControl" />.
    /// </summary>
    public sealed partial class DeleteControl : UserControl
    {

        /// <summary> <see cref = "SaveControl" />'s OKButton. </summary>
        public Windows.UI.Xaml.Controls.Button OKButton { get => this._OKButton.RootButton; set => this._OKButton.RootButton = value; }
        /// <summary> <see cref = "SaveControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton { get => this._CancelButton.RootButton; set => this._CancelButton.RootButton = value; }

        public DeleteControl()
        {
            this.InitializeComponent();
        }
    }
}