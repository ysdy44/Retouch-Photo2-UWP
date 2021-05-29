using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Photos
{
    /// <summary>
    /// Item of <see cref="Photo"/>.
    /// </summary>
    public partial class Photo : UserControl
    {
        //@Delegate
        /// <summary> Occurs when an item in a list view receives an interaction. </summary>
        public static event TypedEventHandler<FrameworkElement, Photo> ItemClick;
        /// <summary> Occurs when the flyout button receives the interaction. </summary>
        public static event TypedEventHandler<FrameworkElement, Photo> FlyoutShow;

        //@Construct
        /// <summary>
        /// Initializes a Photo. 
        /// </summary>
        public Photo()
        {            
            this.InitializeComponent();

            this.FlyoutButton.Tapped += (s, e) =>
            {
                Photo.FlyoutShow?.Invoke(this.RootGrid, this); // Delegate
                e.Handled = true;
            };

            this.PointerPressed += (s, e) =>
            {
                base.CapturePointer(e.Pointer);
            };
            this.PointerReleased += (s, e) =>
            {
                base.ReleasePointerCapture(e.Pointer);
            };

            this.Tapped += (s, e) =>
            {
                Photo.ItemClick?.Invoke(this.RootGrid, this); // Delegate
            };
        }
    }
}