using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "Transformer" />.
    /// </summary>
    public static class TransformerExtensions
    {
        /// <summary> Draw a line. </summary>
        public static void DrawLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1) => ds.DrawLine(point0, point1, Windows.UI.Colors.DodgerBlue);

        /// <summary> Draw a —— </summary>
        public static void DrawThickLine(this CanvasDrawingSession ds, Vector2 vector0, Vector2 vector1)
        {
            ds.DrawLine(vector0, vector1, Windows.UI.Colors.Black, 2);
            ds.DrawLine(vector0, vector1, Windows.UI.Colors.White);
        }

        /// <summary> Draw lines on bound. </summary>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //LTRB: Line
            ds.DrawThickLine(leftTop, rightTop);
            ds.DrawThickLine(rightTop, rightBottom);
            ds.DrawThickLine(rightBottom, leftBottom);
            ds.DrawThickLine(leftBottom, leftTop);
        }

        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //Line
            ds.DrawThickLine(leftTop, rightTop);
            ds.DrawThickLine(rightTop, rightBottom);
            ds.DrawThickLine(rightBottom, leftBottom);
            ds.DrawThickLine(leftBottom, leftTop);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale2
            ds.DrawNode2(leftTop);
            ds.DrawNode2(rightTop);
            ds.DrawNode2(rightBottom);
            ds.DrawNode2(leftBottom);

            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Radian
                ds.DrawThickLine(outsideTop, centerTop);
                ds.DrawNode(outsideTop);

                //Skew
                //ds.DrawNode2(ds, outsideTop);
                //ds.DrawNode2(ds, outsideLeft);
                ds.DrawNode2(outsideRight);
                ds.DrawNode2(outsideBottom);
            }

            //Scale1
            if (Transformer.OutNodeDistance(centerLeft, centerRight))
            {
                ds.DrawNode2(centerTop);
                ds.DrawNode2(centerBottom);
            }
            if (Transformer.OutNodeDistance(centerTop, centerBottom))
            {
                ds.DrawNode2(centerLeft);
                ds.DrawNode2(centerRight);
            }
        }

        /// <summary> Draw a ⊙. </summary>
        public static void DrawNode(this CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Windows.UI.Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Windows.UI.Colors.White);
        }

        /// <summary> Draw a ●. </summary>
        public static void DrawNode2(this CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Windows.UI.Colors.White);
            ds.FillCircle(vector, 6, Windows.UI.Colors.DodgerBlue);
        }

        /// <summary> Draw a ロ. </summary>
        public static void DrawNode3(this CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillRectangle(vector.X - 7, vector.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(vector.X - 6, vector.Y - 6, 12, 12, Windows.UI.Colors.DodgerBlue);
            ds.FillRectangle(vector.X - 5, vector.Y - 5, 10, 10, Windows.UI.Colors.White);
        }

        /// <summary> Draw a ロ. </summary>
        public static void DrawNode4(this CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillRectangle(vector.X - 7, vector.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(vector.X - 6, vector.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(vector.X - 5, vector.Y - 5, 10, 10, Windows.UI.Colors.DodgerBlue);
        }
    }
}