using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Stroke
{
    public static class StrokeExtensions
    {

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
