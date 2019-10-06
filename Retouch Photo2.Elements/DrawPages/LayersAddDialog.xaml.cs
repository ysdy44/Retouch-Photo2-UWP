using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class LayersAddDialog : ContentDialog
    {

        public LayersAddDialog()
        {
            this.InitializeComponent();
            this._CancelButton.Tapped += (s, e) => base.Hide();
        }
    }
}