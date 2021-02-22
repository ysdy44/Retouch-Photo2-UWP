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
                Photo.FlyoutShow?.Invoke(this.Image, this);//Delegate
                e.Handled = true;
            };

            this.RootGrid.PointerEntered += (s, e) => this.Entered();
            this.RootGrid.PointerExited += (s, e) => this.Exited();
            this.RootGrid.PointerPressed += (s, e) => this.Exited();
            this.RootGrid.PointerReleased += (s, e) => this.Exited();
            this.RootGrid.PointerCanceled += (s, e) => this.Exited();

            this.RootGrid.Tapped += (s, e) =>
            {
                Photo.ItemClick?.Invoke(this.Image, this);//Delegate
            };
        }
        
        private void Entered()
        {
            this.ShowStoryboard.Begin();
        }
        private void Exited()
        {
            this.HideStoryboard.Begin();
        }
    }
}