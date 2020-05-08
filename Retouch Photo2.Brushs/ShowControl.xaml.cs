using Microsoft.Graphics.Canvas.Brushes;
using Retouch_Photo2.Elements;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// A control used to show a brush.
    /// </summary>
    public sealed partial class ShowControl : UserControl
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

        /// <summary> Gets or sets the fill-brush. </summary>
        public IBrush FillBrush
        {
            set
            {
                this._vsFillBrush = value;
                this.Invalidate();//Invalidate
            }
        }

        /// <summary> Gets or sets the stroke-brush. </summary>
        public IBrush StrokeBrush
        {
            set
            {
                this._vsStrokeBrush = value;
                this.Invalidate();//Invalidate
            }
        }


        #endregion
        
        //@VisualState
        FillOrStroke _vsFillOrStroke;
        IBrush _vsFillBrush;
        IBrush _vsStrokeBrush;
        public void Invalidate()
        {
            switch (this._vsFillOrStroke)
            {
                case FillOrStroke.Fill:
                    this.Draw(this._vsFillBrush);
                    break;
                case FillOrStroke.Stroke:
                    this.Draw(this._vsStrokeBrush);
                    break;
            }
        }


        //@Construct
        public ShowControl()
        {
            this.InitializeComponent();
        }

        private void Draw(IBrush brush)
        {
            if (brush==null)
            {
                this.Rectangle.Fill = this.NoneBrush;
                return;
            }

            switch (brush.Type)
            {
                case BrushType.None:
                    this.Rectangle.Fill = this.NoneBrush;
                    break;

                case BrushType.Color:
                    this.ColorBrush.Color = brush.Color;
                    this.Rectangle.Fill = this.ColorBrush;
                    break;

                case BrushType.LinearGradient:
                    this.LinearGradientBrush.GradientStops = this.GetStops(brush.Array);
                    this.Rectangle.Fill = this.LinearGradientBrush;
                    break;

                case BrushType.RadialGradient:
                    this.RadialGradientBrush.GradientStops = this.GetStops(brush.Array);
                    this.Rectangle.Fill = this.RadialGradientBrush;
                    break;

                case BrushType.EllipticalGradient:
                    this.EllipticalGradientBrush.GradientStops = this.GetStops(brush.Array);
                    this.Rectangle.Fill = this.EllipticalGradientBrush;
                    break;

                case BrushType.Image:
                    Photo photo = Photo.FindFirstPhoto(brush.Photocopier);
                    this.BitmapImage.UriSource = new Uri(photo.ImageFilePath);
                    this.Rectangle.Fill = this.ImageBrush;
                    break;

                default:
                    break;
            }
        }

        private GradientStopCollection GetStops(CanvasGradientStop[] stops)
        {
            GradientStopCollection gradientStops = new GradientStopCollection();

            foreach (CanvasGradientStop stop in stops)
            {
                GradientStop gradientStop = new GradientStop
                {
                    Color = stop.Color,
                    Offset = stop.Position,
                };
                gradientStops.Add(gradientStop);
            }

            return gradientStops;
        }

    }
}