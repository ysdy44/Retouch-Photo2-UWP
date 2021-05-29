// Core:              ★★
// Referenced:   ★
// Difficult:         ★★★
// Only:              ★★★★
// Complete:      ★★★★
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Retouch_Photo2.Photos
{
    /// <summary>
    /// A Billboard to show the a image and propertys.
    /// </summary>
    public sealed partial class Billboard : UserControl
    {
        double actualWidth = 200;
        double actualHeight = 300;

        #region DependencyProperty

        /// <summary> Gets or sets the <see cref="Billboard"/>'s photo. </summary>
        public Photo Photo
        {
            get => (Photo)base.GetValue(PhotoProperty);
            set => base.SetValue(PhotoProperty, value);
        }
        /// <summary> Identifies the <see cref = "Billboard.Photo" /> dependency property. </summary>
        public static readonly DependencyProperty PhotoProperty = DependencyProperty.Register(nameof(Photo), typeof(Photo), typeof(Billboard), new PropertyMetadata(null, (sender, e) =>
        {
            Billboard control = (Billboard)sender;

            if (e.NewValue is Photo value)
            {
                control.BitmapImage.UriSource = new System.Uri(value.ImageFilePath);
                control.NameTextbolck.Text = value.Name;
                control.SummaryTextBlock.Text = value.ToString();
            }
        }));

        /// <summary> Gets or sets whether <see cref="Billboard"/> is showed. </summary>
        public bool IsShow
        {
            set
            {
                if (value) this.ShowStoryboard.Begin(); // Storyboard
                else this.HideStoryboard.Begin(); // Storyboard
            }
        }

        #endregion


        //@Construct
        /// <summary>
        /// Initializes a Billboard. 
        /// </summary>
        public Billboard()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this.actualWidth = e.NewSize.Width;
                this.actualHeight = e.NewSize.Height;
            };
        }

        public void CalculatePostion(FrameworkElement placementTarget)
        {
            Point buttonPostion = placementTarget.TransformToVisual(Window.Current.Content).TransformPoint(new Point(0, 0));

            switch (base.FlowDirection)
            {
                case FlowDirection.LeftToRight:
                    break;
                case FlowDirection.RightToLeft:
                    buttonPostion.X = Window.Current.Bounds.Width - buttonPostion.X;
                    break;
                default:
                    break;
            }

            double centerCoordsX = buttonPostion.X + placementTarget.ActualWidth / 2;
            double centerCoordsY = buttonPostion.Y + placementTarget.ActualHeight / 2;

            double x = centerCoordsX - this.actualWidth / 2;
            double y = centerCoordsY - (this.actualHeight - 70) / 2;

            if (x < 0) x = 0;
            if (y < 0) y = 0;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

    }
}