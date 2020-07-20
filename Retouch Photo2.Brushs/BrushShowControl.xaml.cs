using HSVColorPickers;
using Retouch_Photo2.Elements;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public sealed partial class BrushShowControl : UserControl
    {

        #region DependencyProperty


        /// <summary> Gets or sets the fill or stroke. </summary>
        public FillOrStroke FillOrStroke
        {
            set
            {
                this._vsFillOrStroke = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the fill. </summary>
        public IBrush Fill
        {
            set
            {
                this._vsFill = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the stroke. </summary>
        public IBrush Stroke
        {
            set
            {
                this._vsStroke = value;
                this.Invalidate();//Invalidate
            }
        }


        #endregion
        
        //@VisualState
        FillOrStroke _vsFillOrStroke;
        IBrush _vsFill;
        IBrush _vsStroke;
        /// <summary>
        /// Invalidate.
        /// </summary>
        public void Invalidate()
        {
            switch (this._vsFillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Rectangle.Fill = this.ToBrush(this._vsFill);
                    break;
                case FillOrStroke.Stroke:
                    this.Rectangle.Fill = this.ToBrush(this._vsStroke);
                    break;
            }
        }


        //@Construct
        /// <summary>
        /// Initializes a BrushShowControl. 
        /// </summary>
        public BrushShowControl()
        {
            this.InitializeComponent();
        }

        private Brush ToBrush(IBrush brush)
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