using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Pages.MainPages
{
    /// <summary>
    /// <see cref = "MainPage" /> Appbar's <see cref = "DeleteControl" />.
    /// </summary>
    public sealed partial class DeleteControl : UserControl
    {
        //@Content
        /// <summary> <see cref = "SaveControl" />'s OKButton. </summary>
        public Windows.UI.Xaml.Controls.Button OKButton => this._OKButton.RootButton;
        /// <summary> <see cref = "SaveControl" />'s CancelButton. </summary>
        public Windows.UI.Xaml.Controls.Button CancelButton => this._CancelButton.RootButton;

        //@Construct
        public DeleteControl()
        {
            this.InitializeComponent();
        }
    }
}