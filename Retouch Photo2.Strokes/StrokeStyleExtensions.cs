using Microsoft.Graphics.Canvas.Geometry;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Retouch_Photo2.Stroke
{
    /// <summary>
    /// Extensions of <see cref="CanvasStrokeStyle"/>.
    /// </summary>
    public static class StrokeStyleExtensions
    {

        /// <summary>
        /// Turn into stroke style.
        /// </summary>
        /// <param name="shape"> The destination shape. </param>
        /// <param name="strokeStyle"> The stroke style style. </param>
        public static void SetStrokeStyle(this Shape shape, CanvasStrokeStyle strokeStyle)
        {
            if (strokeStyle == null) return;

            shape.StrokeDashArray = strokeStyle.DashStyle.ToDoubleCollection();
            shape.StrokeDashCap = strokeStyle.DashCap.ToPenLineCap();
            shape.StrokeStartLineCap = strokeStyle.StartCap.ToPenLineCap();
            shape.StrokeEndLineCap = strokeStyle.EndCap.ToPenLineCap();
            shape.StrokeDashOffset = strokeStyle.DashOffset;
        }

        /// <summary>
        /// Turn into pen line cap.
        /// </summary>
        /// <param name="cap"> The cap. </param>     
        /// <returns> The product cap. </returns>
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

        /// <summary>
        /// Turn into DoubleCollection.
        /// </summary>
        /// <param name="style"> The style. </param>
        /// <returns> The product DoubleCollection. </returns>
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


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <param name="strokeStyle"> The stroke style. </param>
        /// <returns> The cloned <see cref="CanvasStrokeStyle"/>. </returns>
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

        /// <summary>
        /// Copy with self.
        /// </summary>
        /// <param name="strokeStyle"> The stroke style. </param>
        /// <param name="other"> The other. </param>
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
