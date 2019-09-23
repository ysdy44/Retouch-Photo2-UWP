using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Elements
{
    public sealed partial class LayersAddDialog : ContentDialog
    {
        //@Content
        /// <summary> PhotoButton. </summary>
        public Button PhotoButton => this._PhotoButton;
        /// <summary> DestopButton. </summary>
        public Button DestopButton => this._DestopButton;

        public LayersAddDialog()
        {
            this.InitializeComponent();
            this._CancelButton.Tapped += (s, e) => base.Hide();
        }
    }
}