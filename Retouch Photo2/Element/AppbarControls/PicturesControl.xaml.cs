using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Retouch_Photo2.Element.AppbarControls
{
    public sealed partial class PicturesControl : UserControl
    {
        //Delegate
        public delegate void PicturesEventHandler(PickerLocationId location);
        public event PicturesEventHandler PicturesPicker;
        public event TappedEventHandler CancelButtonTapped;

        public PicturesControl()
        {
            this.InitializeComponent();
            this.PhotoButton.Tapped += (s, e) => this.PicturesPicker?.Invoke(PickerLocationId.PicturesLibrary);
            this.DestopButton.Tapped += (s, e) => this.PicturesPicker?.Invoke(PickerLocationId.Desktop);
            this.CancelButton.Tapped += (s, e) => this.CancelButtonTapped?.Invoke(s, e);
        }
    }
}
