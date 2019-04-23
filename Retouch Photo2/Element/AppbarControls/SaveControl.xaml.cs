using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Element.AppbarControls
{
    public sealed partial class SaveControl : UserControl
    {
        //Delegate
        public event TappedEventHandler OKButtonTapped;
        public event TappedEventHandler CancelButtonTapped;

        public SaveControl()
        {
            this.InitializeComponent();

            this.OKButton.Tapped += (s, e) => this.OKButtonTapped?.Invoke(s, e);
            this.CancelButton.Tapped += (s, e) => this.CancelButtonTapped?.Invoke(s, e);
        }
    }
}
