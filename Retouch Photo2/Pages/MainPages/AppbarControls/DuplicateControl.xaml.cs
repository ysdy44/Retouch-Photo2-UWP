using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Pages.MainPages.AppbarControls
{
    public sealed partial class DuplicateControl : UserControl
    {
        //Delegate
        public event TappedEventHandler OKButtonTapped;
        public event TappedEventHandler CancelButtonTapped;

        public DuplicateControl()
        {
            this.InitializeComponent();

            this.OKButton.Tapped += (s, e) => this.OKButtonTapped?.Invoke(s, e);
            this.CancelButton.Tapped += (s, e) => this.CancelButtonTapped?.Invoke(s, e);
        }
    }
}
