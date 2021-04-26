// Core:              ★★
// Referenced:   ★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Photos
{
    /// <summary>
    /// Represents a canvas control, that containing some <see cref="Retouch_Photo2.Photos.Billboard"/>.
    /// </summary>
    public class PhotoBillboardCanvas : Canvas
    {

        private Billboard Billboard { get; } = new Billboard();
        private bool IsOverlayDismiss
        {
            set
            {
                if (value)
                    this.Background = new SolidColorBrush(Colors.Transparent);
                else
                    this.Background = null;
            }
        }

        //@Construct
        /// <summary>
        /// Initializes a BillboardCanvas. 
        /// </summary>
        public PhotoBillboardCanvas()
        {
            this.IsHitTestVisible = false;
            this.Children.Add(this.Billboard);
            this.Tapped += (s, e) =>
            {
                this.Billboard.IsShow = this.IsHitTestVisible = this.IsOverlayDismiss = false;
            };
        }

        public void Show(FrameworkElement element, Photo photo)
        {
            this.Billboard.CalculatePostion(element);
            this.Billboard.Photo = photo;

            this.Billboard.IsShow = this.IsHitTestVisible = this.IsOverlayDismiss = true;
        }
    }
}