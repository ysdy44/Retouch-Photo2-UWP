using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "Transformer" />.
    /// </summary>
    public static class TransformerExtensions
    {

        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1) => ds.DrawLine(point0, point1, Windows.UI.Colors.DodgerBlue);
        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1, Windows.UI.Color accentColor) => ds.DrawLine(point0, point1, accentColor);


        /// <summary>
        /// Draw a ——.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawThickLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1)
        {
            ds.DrawLine(point0, point1, Windows.UI.Colors.Black, 2);
            ds.DrawLine(point0, point1, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop);
            ds.DrawLine(rightTop, rightBottom);
            ds.DrawLine(rightBottom, leftBottom);
            ds.DrawLine(leftBottom, leftTop);
        }
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop, accentColor);
            ds.DrawLine(rightTop, rightBottom, accentColor);
            ds.DrawLine(rightBottom, leftBottom, accentColor);
            ds.DrawLine(leftBottom, leftTop, accentColor);
        }


        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
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
        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix,Windows.UI.Color accentColor, bool disabledRadian = false)
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
            ds.DrawNode2(leftTop, accentColor);
            ds.DrawNode2(rightTop, accentColor);
            ds.DrawNode2(rightBottom, accentColor);
            ds.DrawNode2(leftBottom, accentColor);

            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Radian
                ds.DrawThickLine(outsideTop, centerTop);
                ds.DrawNode(outsideTop, accentColor);

                //Skew
                //ds.DrawNode2(ds, outsideTop, accentColor);
                //ds.DrawNode2(ds, outsideLeft, accentColor);
                ds.DrawNode2(outsideRight, accentColor);
                ds.DrawNode2(outsideBottom, accentColor);
            }

            //Scale1
            if (Transformer.OutNodeDistance(centerLeft, centerRight))
            {
                ds.DrawNode2(centerTop, accentColor);
                ds.DrawNode2(centerBottom, accentColor);
            }
            if (Transformer.OutNodeDistance(centerTop, centerBottom))
            {
                ds.DrawNode2(centerLeft, accentColor);
                ds.DrawNode2(centerRight, accentColor);
            }
        }


        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.DodgerBlue);
            ds.FillCircle(point, 6, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, accentColor);
            ds.FillCircle(point, 6, Windows.UI.Colors.White);
        }
        

        /// <summary>
        /// Draw a ●.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode2(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.White);
            ds.FillCircle(point, 6, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ●.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode2(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.White);
            ds.FillCircle(point, 6, accentColor);
        }


        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode3(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.DodgerBlue);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode3(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, accentColor);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode4(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode4(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, accentColor);
        }
    }
}