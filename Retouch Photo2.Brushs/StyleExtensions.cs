using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Brushs
{
    public static class StyleExtensions
    {

        //Brush
        public static Brush ToBrush(this IBrush brush)
        {
            if (brush == null) return null;

            switch (brush.Type)
            {
                case BrushType.None: return null;

                case BrushType.Color: return new SolidColorBrush(brush.Color);

                case BrushType.LinearGradient: return new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0.5),
                        EndPoint = new Point(1, 0.5),
                        GradientStops = brush.Array.ToStops()
                    };

                case BrushType.RadialGradient: return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.2,
                        RadiusY = 0.6,
                        GradientStops = brush.Array.ToStops()
                    };

                case BrushType.EllipticalGradient: return new RadialGradientBrush
                    {
                        Center = new Point(0.5, 0.5),
                        GradientOrigin = new Point(0.5, 0.5),
                        RadiusX = 0.5,
                        RadiusY = 0.5,
                        GradientStops = brush.Array.ToStops()
                    };

                default: return null;
            }
        }

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


        //StrokeStyle
        public static void SetStrokeStyle(this Shape shape, CanvasStrokeStyle strokeStyle)
        {
            if (strokeStyle == null) return;

            shape.StrokeDashArray = strokeStyle.DashStyle.ToDoubleCollection();
            shape.StrokeDashCap = strokeStyle.DashCap.ToPenLineCap();
            shape.StrokeStartLineCap = strokeStyle.StartCap.ToPenLineCap();
            shape.StrokeEndLineCap = strokeStyle.EndCap.ToPenLineCap();
            shape.StrokeDashOffset = strokeStyle.DashOffset;
        }
        
        public static PenLineCap ToPenLineCap(this CanvasCapStyle cap)
        {
            switch (cap)
            {
                case CanvasCapStyle.Flat: return PenLineCap.Flat;
                case CanvasCapStyle.Square: return PenLineCap.Square;
                case CanvasCapStyle.Round: return PenLineCap.Round;
                case CanvasCapStyle.Triangle: return PenLineCap.Triangle;
                default: return PenLineCap.Flat;
            }
        }

        public static DoubleCollection ToDoubleCollection(this CanvasDashStyle style)
        {
            switch (style)
            {
                case CanvasDashStyle.Solid: return null;
                case CanvasDashStyle.Dash: return new DoubleCollection { 2, 2 };
                case CanvasDashStyle.Dot: return new DoubleCollection { 0, 2 };
                case CanvasDashStyle.DashDot: return new DoubleCollection { 2, 2, 0, 2 };
                case CanvasDashStyle.DashDotDot: return new DoubleCollection { 2, 2, 0, 2, 0, 2 };
                default: return null;
            }
        }


        //Clone
        public static CanvasStrokeStyle Clone(this CanvasStrokeStyle strokeStyle)
        {
            if (strokeStyle == null) return new CanvasStrokeStyle();
            return new CanvasStrokeStyle
            {
                DashStyle = strokeStyle.DashStyle,
                DashCap = strokeStyle.DashCap,
                StartCap = strokeStyle.StartCap,
                EndCap = strokeStyle.EndCap,
                DashOffset = strokeStyle.DashOffset,
                               
                MiterLimit = strokeStyle.MiterLimit,
                LineJoin = strokeStyle.LineJoin,

                CustomDashStyle = strokeStyle.CustomDashStyle,
                TransformBehavior = strokeStyle.TransformBehavior,
            };
        }

        public static void CopyWith(this CanvasStrokeStyle strokeStyle, CanvasStrokeStyle other)
        {
            if (strokeStyle == null) return;

            strokeStyle.DashStyle = other.DashStyle;
            strokeStyle.DashCap = other.DashCap;
            strokeStyle.StartCap = other.StartCap;
            strokeStyle.EndCap = other.EndCap;
            strokeStyle.DashOffset = other.DashOffset;

            strokeStyle.MiterLimit = other.MiterLimit;
            strokeStyle.LineJoin = other.LineJoin;

            strokeStyle.CustomDashStyle = other.CustomDashStyle;
            strokeStyle.TransformBehavior = other.TransformBehavior;
        }

    }
}