using Microsoft.Graphics.Canvas.Geometry;

namespace Retouch_Photo2.Brushs
{
    public static class StrokeExtensions
    {
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