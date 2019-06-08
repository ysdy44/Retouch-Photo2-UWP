using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define Transformer. </summary>
    public partial struct Transformer
    {

        /// <summary> Draw lines on bound. </summary>
        public static void DrawBound(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Windows.UI.Colors.DodgerBlue);
        }

        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBoundNodes(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            //Line
            ds.DrawLine(leftTop, rightTop, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Windows.UI.Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Windows.UI.Colors.DodgerBlue);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale2
            Transformer.DrawNode2(ds, leftTop);
            Transformer.DrawNode2(ds, rightTop);
            Transformer.DrawNode2(ds, rightBottom);
            Transformer.DrawNode2(ds, leftBottom);

            //Scale1
            if (Transformer.OutNodeDistance(centerLeft, centerRight))
            {
                Transformer.DrawNode2(ds, centerTop);
                Transformer.DrawNode2(ds, centerBottom);
            }
            if (Transformer.OutNodeDistance(centerTop, centerBottom))
            {
                Transformer.DrawNode2(ds, centerLeft);
                Transformer.DrawNode2(ds, centerRight);
            }

            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Radian
                ds.DrawLine(outsideTop, centerTop, Windows.UI.Colors.DodgerBlue);
                Transformer.DrawNode(ds, outsideTop);

                //Skew
                //Transformer.DrawNode2(ds, outsideTop);
                //Transformer.DrawNode2(ds, outsideLeft);
                Transformer.DrawNode2(ds, outsideRight);
                Transformer.DrawNode2(ds, outsideBottom);
            }
        }



        /// <summary> Draw a ⊙. </summary>
        public static void DrawNode(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Windows.UI.Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Windows.UI.Colors.White);
        }

        /// <summary> Draw a ●. </summary>
        public static void DrawNode2(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Windows.UI.Colors.White);
            ds.FillCircle(vector, 6, Windows.UI.Colors.DodgerBlue);
        }

        /// <summary>
        /// draw a ロ
        /// </summary>
        public static void DrawNode3(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillRectangle(vector.X - 7, vector.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(vector.X - 6, vector.Y - 6, 12, 12, Windows.UI.Colors.DodgerBlue);
            ds.FillRectangle(vector.X - 5, vector.Y - 5, 10, 10, Windows.UI.Colors.White);
        }

        /// <summary>
        /// draw a ロ
        /// </summary>
        public static void DrawNode4(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillRectangle(vector.X - 7, vector.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(vector.X - 6, vector.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(vector.X - 5, vector.Y - 5, 10, 10, Windows.UI.Colors.DodgerBlue);
        }



        /// <summary> Draw a —— </summary>
        public static void DrawLine(CanvasDrawingSession ds, Vector2 vector0, Vector2 vector1)
        {
            ds.DrawLine(vector0, vector1, Windows.UI.Colors.Black, 3);
            ds.DrawLine(vector0, vector1, Windows.UI.Colors.White);
        }
    }
}
