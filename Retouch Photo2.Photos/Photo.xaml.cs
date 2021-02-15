using Windows.Foundation;
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
        public static event TypedEventHandler<object, Photo> ItemClick;
        /// <summary> Occurs when the flyout button receives the interaction. </summary>
        public static event TypedEventHandler<object, Photo> FlyoutShow;

        //@Construct
        /// <summary>
        /// Initializes a Photo. 
        /// </summary>
        public Photo()
        {            
            this.InitializeComponent();

            this.Image.SizeChanged += (s, e) =>
            {
                if (e.PreviousSize == e.NewSize) return;

                this.BackgroundRectangle.Width = e.NewSize.Width;
                this.BackgroundRectangle.Height = e.NewSize.Height;
            };

            this.FlyoutButton.Tapped += (s, e) =>
            {
                UserControl userControl = this;
                Photo.FlyoutShow?.Invoke(userControl, this);//Delegate
                e.Handled = true;
            };

            this.RootGrid.PointerEntered += (s, e) => this.Entered();
            this.RootGrid.PointerExited += (s, e) => this.Exited();
            this.RootGrid.PointerPressed += (s, e) => this.Exited();
            this.RootGrid.PointerReleased += (s, e) => this.Exited();
            this.RootGrid.PointerCanceled += (s, e) => this.Exited();

            this.RootGrid.Tapped += (s, e) =>
            {
                Photo.ItemClick?.Invoke(this, this);//Delegate
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