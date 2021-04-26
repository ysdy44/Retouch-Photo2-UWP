// Core:              ★★
// Referenced:   ★★
// Difficult:         ★★★★
// Only:              ★★★
// Complete:      ★★★★
using HSVColorPickers;
using Retouch_Photo2.Photos;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public partial class ShowControl : UserControl
    {

        protected Brush RectangleFill { set => this.Rectangle.Fill = value; }

        //@Construct
        /// <summary>
        /// Initializes a ShowControl. 
        /// </summary>
        public ShowControl()
        {
            this.InitializeComponent();
        }

        protected Brush ToBrush(IBrush brush)
        {
            if (brush==null) return this.NoneBrush;

            switch (brush.Type)
            {
                case BrushType.None: return this.NoneBrush;

                case BrushType.Color:
                    this.ColorBrush.Color = brush.Color;
                  return this.ColorBrush;

                case BrushType.LinearGradient:
                    this.LinearGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.LinearGradientBrush;

                case BrushType.RadialGradient:
                    this.RadialGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.RadialGradientBrush;

                case BrushType.EllipticalGradient:
                    this.EllipticalGradientBrush.GradientStops = brush.Stops.ToStops();
                    return this.EllipticalGradientBrush;

                case BrushType.Image:
                    Photo photo = Photo.FindFirstPhoto(brush.Photocopier);
                    this.BitmapImage.UriSource = new Uri(photo.ImageFilePath);
                    return this.ImageBrush;

                default: return this.NoneBrush;
            }
        }

    }
}