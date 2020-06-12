using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Extensions of <see cref = "IBrush" />.
    /// </summary>
    public static class BrushExtensions
    {

        /// <summary>
        /// Turn into brush.
        /// </summary>
        /// <param name="brush"> The source brush. </param>
        /// <returns> The product brush. </returns>
        public static Brush ToBrush(this IBrush brush)
        {
            if (brush == null) return null;

            switch (brush.Type)
            {
                case BrushType.None: return null;

                case BrushType.Color: return new SolidColorBrush(brush.Color);

                case BrushType.LinearGradient:
                    return new LinearGradientBrush
                    {
                        StartPoint = new Point(0.5, 0),
                        EndPoint = new Point(0.5, 1),
                        GradientStops = brush.Stops.ToStops()
                    };

                case BrushType.RadialGradient:
                    return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.5,
                        RadiusY = 0.5,
                        GradientStops = brush.Stops.ToStops()
                    };

                case BrushType.EllipticalGradient:
                    return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.2,
                        RadiusY = 0.6,
                        GradientStops = brush.Stops.ToStops()
                    };

                default: return null;
            }
        }


        /// <summary>
        /// Turn into weight brush.
        /// </summary>
        /// <param name="brush"> The source brush. </param>
        /// <returns> The product brush. </returns>
        public static Brush ToWeightBrush(this IBrush brush)
        {
            if (brush == null) return null;

            switch (brush.Type)
            {
                case BrushType.None: return null;

                case BrushType.Color: return new SolidColorBrush(brush.Color);

                case BrushType.LinearGradient:
                    return new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0.5),
                        EndPoint = new Point(1, 0.5),
                        GradientStops = brush.Stops.ToStops()
                    };

                case BrushType.RadialGradient:
                    return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.2,
                        RadiusY = 0.6,
                        GradientStops = brush.Stops.ToStops()
                    };

                case BrushType.EllipticalGradient:
                    return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.5,
                        RadiusY = 0.5,
                        GradientStops = brush.Stops.ToStops()
                    };

                default: return null;
            }
        }


        /// <summary>
        /// Turn into GradientStopCollection.
        /// </summary>
        /// <param name="stops"> The source stops. </param>
        /// <returns> The product GradientStopCollection. </returns>
        public static GradientStopCollection ToStops(this CanvasGradientStop[] stops)
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