using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    public class HomographyController
    {

        /// <summary> Scaling around the center. </summary>
        public static bool IsCenter => App.ViewModel.KeyCtrl;
        /// <summary> Maintain a ratio when scaling. </summary>
        public static bool IsRatio => App.ViewModel.KeyShift;
        /// <summary> Step Frequency when spinning. </summary>
        public static bool IsStepFrequency => App.ViewModel.KeyShift;


        //---------------------------------------------------------Transformer--------------------------------------------------------//


        /// <summary> The nodes mode of [Transformer]. </summary>
        public enum TransformerMode
        {
            None,
            Translation,
            Rotation,

            SkewLeft,
            SkewTop,
            SkewRight,
            SkewBottom,

            ScaleLeft,
            ScaleTop,
            ScaleRight,
            ScaleBottom,

            ScaleLeftTop,
            ScaleRightTop,
            ScaleRightBottom,
            ScaleLeftBottom,
        }


        /// <summary> Define Transformer. </summary>
        public struct Transformer
        {

            #region Transformer


            //Source
            public Vector2 SrcLeftTop { get; private set; }
            public Vector2 SrcRightTop { get; private set; }
            public Vector2 SrcRightBottom { get; private set; }
            public Vector2 SrcLeftBottom { get; private set; }

            //Destination 
            public Vector2 DstLeftTop { get; set; }
            public Vector2 DstRightTop { get; set; }
            public Vector2 DstRightBottom { get; set; }
            public Vector2 DstLeftBottom { get; set; }

            public bool DdisabledRadian;

            public Matrix3x2 Matrix => Transformer.FindHomography(this.SrcLeftTop, this.SrcRightTop, this.SrcRightBottom, this.SrcLeftBottom, this.DstLeftTop, this.DstRightTop, this.DstRightBottom, this.DstLeftBottom);
            public Matrix3x2 InverseMatrix => Transformer.FindHomography(this.DstLeftTop, this.DstRightTop, this.DstRightBottom, this.DstLeftBottom, this.SrcLeftTop, this.SrcRightTop, this.SrcRightBottom, this.SrcLeftBottom);

            public static Transformer CreateFromVector(Vector2 leftTop, Vector2 rightBottom) => new Transformer
            {
                //Source
                SrcLeftTop = leftTop,
                SrcRightTop = new Vector2(rightBottom.X, leftTop.Y),
                SrcRightBottom = rightBottom,
                SrcLeftBottom = new Vector2(leftTop.X, rightBottom.Y),
                //Destination
                DstLeftTop = leftTop,
                DstRightTop = new Vector2(rightBottom.X, leftTop.Y),
                DstRightBottom = rightBottom,
                DstLeftBottom = new Vector2(leftTop.X, rightBottom.Y),
            };
            public static Transformer CreateFromSize(float width, float height, Vector2 postion, float scale = 1, bool disabledRadian = false) => new Transformer
            {
                //Source
                SrcLeftTop = Vector2.Zero,
                SrcRightTop = new Vector2(width, 0),
                SrcRightBottom = new Vector2(width, height),
                SrcLeftBottom = new Vector2(0, height),
                //Destination
                DstLeftTop = postion,
                DstRightTop = postion + new Vector2(width * scale, 0),
                DstRightBottom = postion + new Vector2(width * scale, height * scale),
                DstLeftBottom = postion + new Vector2(0, height * scale),

                DdisabledRadian = disabledRadian
            };
            public static void CopyWith(Layer layer, Transformer transformer)
            {
                layer.Transformer.DstLeftTop = transformer.DstLeftTop;
                layer.Transformer.DstRightTop = transformer.DstRightTop;
                layer.Transformer.DstRightBottom = transformer.DstRightBottom;
                layer.Transformer.DstLeftBottom = transformer.DstLeftBottom;

                layer.Transformer.DdisabledRadian = transformer.DdisabledRadian;
            }

            public Vector2 DstLeft => (this.DstLeftTop + this.DstLeftBottom) / 2;
            public Vector2 DstTop => (this.DstLeftTop + this.DstRightTop) / 2;
            public Vector2 DstRight => (this.DstRightTop + this.DstRightBottom) / 2;
            public Vector2 DstBottom => (this.DstRightBottom + this.DstLeftBottom) / 2;

            public Vector2 DstCenter => (this.DstLeftTop + this.DstRightBottom) / 2;

            public float DstMinX => Math.Min(Math.Min(this.DstLeftTop.X, this.DstRightTop.X), Math.Min(this.DstRightBottom.X, this.DstLeftBottom.X));
            public float DstMaxX => Math.Max(Math.Max(this.DstLeftTop.X, this.DstRightTop.X), Math.Max(this.DstRightBottom.X, this.DstLeftBottom.X));
            public float DstMinY => Math.Min(Math.Min(this.DstLeftTop.Y, this.DstRightTop.Y), Math.Min(this.DstRightBottom.Y, this.DstLeftBottom.Y));
            public float DstMaxY => Math.Max(Math.Max(this.DstLeftTop.Y, this.DstRightTop.Y), Math.Max(this.DstRightBottom.Y, this.DstLeftBottom.Y));


            /// <summary> Multiplies matrix to layer's transformer. </summary>
            public static void Add(Layer layer, Transformer startTransformer, Vector2 vector)
            {
                layer.Transformer.DstLeftTop = startTransformer.DstLeftTop + vector;
                layer.Transformer.DstRightTop = startTransformer.DstRightTop + vector;
                layer.Transformer.DstRightBottom = startTransformer.DstRightBottom + vector;
                layer.Transformer.DstLeftBottom = startTransformer.DstLeftBottom + vector;
            }
            /// <summary> Multiplies vector to layer's ttransformer. </summary>
            public static void Multiplies(Layer layer, Transformer startTransformer, Matrix3x2 matrix)
            {
                layer.Transformer.DstLeftTop = Vector2.Transform(startTransformer.DstLeftTop, matrix);
                layer.Transformer.DstRightTop = Vector2.Transform(startTransformer.DstRightTop, matrix);
                layer.Transformer.DstRightBottom = Vector2.Transform(startTransformer.DstRightBottom, matrix);
                layer.Transformer.DstLeftBottom = Vector2.Transform(startTransformer.DstLeftBottom, matrix);
            }
            #endregion


            #region Homography


            /// <summary> Find Homography. </summary>
            public static Matrix3x2 FindHomography(Vector2 srcLeftTop, Vector2 srcRightTop, Vector2 srcRightBottom, Vector2 srcLeftBottom, Vector2 dstLeftTop, Vector2 dstRightTop, Vector2 dstRightBottom, Vector2 dstLeftBottom)
            {
                float x0 = srcLeftTop.X, x1 = srcRightTop.X, x2 = srcLeftBottom.X, x3 = srcRightBottom.X;
                float y0 = srcLeftTop.Y, y1 = srcRightTop.Y, y2 = srcLeftBottom.Y, y3 = srcRightBottom.Y;
                float u0 = dstLeftTop.X, u1 = dstRightTop.X, u2 = dstLeftBottom.X, u3 = dstRightBottom.X;
                float v0 = dstLeftTop.Y, v1 = dstRightTop.Y, v2 = dstLeftBottom.Y, v3 = dstRightBottom.Y;
                float[,] A = new float[8, 9]
                {
                    { x0, y0, 1, 0, 0, 0, -x0* u0, -y0* u0, u0 },
                    { x1, y1, 1, 0, 0, 0, -x1* u1, -y1* u1, u1 },
                    { x2, y2, 1, 0, 0, 0, -x2* u2, -y2* u2, u2 },
                    { x3, y3, 1, 0, 0, 0, -x3* u3, -y3* u3, u3 },
                    { 0, 0, 0, x0, y0, 1, -x0* v0, -y0* v0, v0 },
                    { 0, 0, 0, x1, y1, 1, -x1* v1, -y1* v1, v1 },
                    { 0, 0, 0, x2, y2, 1, -x2* v2, -y2* v2, v2 },
                    { 0, 0, 0, x3, y3, 1, -x3* v3, -y3* v3, v3 },
                };


                //epu:A's row  var:A's col-1
                int row, column;
                for (row = 0, column = 0; column < 8 && row < 8; column++, row++)
                {
                    int max_r = row;
                    for (int i = row + 1; i < 8; i++)
                    {
                        if ((1e-12) < Math.Abs(A[i, column]) - Math.Abs(A[max_r, column]))
                        {
                            max_r = i;
                        }
                    }

                    if (max_r != row)
                    {
                        for (int j = 0; j < 8 + 1; j++)
                        {
                            var a = A[row, j];
                            var b = A[max_r, j];
                            A[row, j] = b;
                            A[max_r, j] = a;
                        }
                    }

                    for (int i = row + 1; i < 8; i++)
                    {
                        if (Math.Abs(A[i, column]) < (1e-12)) continue;

                        float tmp = -A[i, column] / A[row, column];

                        for (int j = column; j < 8 + 1; j++)
                        {
                            A[i, j] += tmp * A[row, j];
                        }
                    }
                }


                float[] ret = new float[6];
                for (int i = 8 - 1; i >= 0; i--)
                {
                    //Calculate Unique Solutions
                    float tmp = 0;
                    for (int j = i + 1; j < 8; j++)
                    {
                        float n = ret[j % 6];
                        tmp += A[i, j] * n;
                    }
                    ret[i % 6] = (A[i, 8] - tmp) / A[i, i];
                }

                return new Matrix3x2
                (
                    m11: ret[0], m12: ret[3],
                    m21: ret[1], m22: ret[4],
                    m31: ret[2], m32: ret[5]
                );
            }

            /*
           Previous Code:

            /// <summary> Find Homography. </summary>
            public static Matrix3x2 FindHomography(Vector2 SrcLeftTop, Vector2 SrcRightTop, Vector2 SrcRightBottom, Vector2 SrcLeftBottom, Vector2 DstLeftTop, Vector2 DstRightTop, Vector2 DstRightBottom, Vector2 DstLeftBottom)
            {
                var src = new Vector2[]
                {
                     SrcLeftTop,
                     SrcRightTop,
                     SrcRightBottom,
                     SrcLeftBottom,
                };

                var dst = new Vector2[]
                {
                     DstLeftTop,
                     DstRightTop,
                     DstRightBottom,
                     DstLeftBottom,
                };

                float[] ret = new float[6];

                fixed (Vector2* s = src, d = dst)
                {
                    fixed (float* r = ret)
                    {
                        Transformer.GetPerspectiveTransform(s, d, r);
                    }
                }

                float m11 = ret[0];
                float m12 = ret[3];
                float m21 = ret[1];
                float m22 = ret[4];
                float offsetX = ret[2];
                float offsetY = ret[5];
                return new Matrix3x2(m11, m12, m21, m22, offsetX, offsetY);
            }

            private static void Gauss(float[,] A, int equ, int var, float* ans)
            {
                //epu:A's row  var:A's col-1
                int row, col;
                for (row = 0, col = 0; col < var && row < equ; col++, row++)
                {
                    int max_r = row;
                    for (int i = row + 1; i < equ; i++)
                    {
                        if ((1e-12) < Math.Abs(A[i, col]) - Math.Abs(A[max_r, col]))
                        {
                            max_r = i;
                        }
                    }

                    if (max_r != row)
                    {
                        for (int j = 0; j < var + 1; j++)
                        {
                            var a = A[row, j];
                            var b = A[max_r, j];
                            A[row, j] = b;
                            A[max_r, j] = a;
                        }
                    }

                    for (int i = row + 1; i < equ; i++)
                    {
                        if (Math.Abs(A[i, col]) < (1e-12))
                            continue;

                        float tmp = -A[i, col] / A[row, col];

                        for (int j = col; j < var + 1; j++)
                        {
                            A[i, j] += tmp * A[row, j];
                        }
                    }

                }

                for (int i = var - 1; i >= 0; i--)
                {
                    //计算唯一解。
                    //Calculate Unique Solutions
                    float tmp = 0;
                    for (int j = i + 1; j < var; j++)
                    {
                        tmp += A[i, j] * (*(ans + j));
                    }
                    ans[i] = (A[i, var] - tmp) / A[i, i];
                }
            }

            private static void GetPerspectiveTransform(Vector2* src, Vector2* dst, float* ret)
            {
                float x0 = src[0].X, x1 = src[1].X, x2 = src[3].X, x3 = src[2].X;
                float y0 = src[0].Y, y1 = src[1].Y, y2 = src[3].Y, y3 = src[2].Y;
                float u0 = dst[0].X, u1 = dst[1].X, u2 = dst[3].X, u3 = dst[2].X;
                float v0 = dst[0].Y, v1 = dst[1].Y, v2 = dst[3].Y, v3 = dst[2].Y;
                float[,] A = new float[8, 9]
                {
                { x0, y0, 1, 0, 0, 0, -x0* u0, -y0* u0, u0 },
                { x1, y1, 1, 0, 0, 0, -x1* u1, -y1* u1, u1 },
                { x2, y2, 1, 0, 0, 0, -x2* u2, -y2* u2, u2 },
                { x3, y3, 1, 0, 0, 0, -x3* u3, -y3* u3, u3 },
                { 0, 0, 0, x0, y0, 1, -x0* v0, -y0* v0, v0 },
                { 0, 0, 0, x1, y1, 1, -x1* v1, -y1* v1, v1 },
                { 0, 0, 0, x2, y2, 1, -x2* v2, -y2* v2, v2 },
                { 0, 0, 0, x3, y3, 1, -x3* v3, -y3* v3, v3 },
                };

                Transformer.Gauss(A, 8, 8, ret);

                *(ret + 8) = 1;
            }
                 */


            #endregion


            #region Contains        


            /// <summary> Radius of node'. Defult 12. </summary>
            public static float NodeRadius = 12.0f;
            /// <summary> Whether the distance exceeds [NodeRadius]. Defult: 144. </summary>
            public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;


            /// <summary> Minimum distance between two nodes. Defult 20. </summary>
            public static float NodeDistance = 20.0f;
            /// <summary> Double [NodeDistance]. Defult 40. </summary>
            public static float NodeDistanceDouble = 40.0f;
            /// <summary> Whether the distance'LengthSquared exceeds [NodeDistance]. Defult: 400. </summary>
            public static bool OutNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;


            /// <summary> Get outside node. </summary>
            public static Vector2 OutsideNode(Vector2 nearNode, Vector2 farNode) => nearNode - Vector2.Normalize(farNode - nearNode) * Transformer.NodeDistanceDouble;


            /// <summary> Returns whether the area filled by the bound rect contains the specified point. </summary>
            public static bool ContainsBound(Vector2 point, Transformer transformer)
            {
                Vector2 v = Vector2.Transform(point, transformer.InverseMatrix);
                return v.X > 0 && v.X < transformer.SrcRightBottom.X && v.Y > 0 && v.Y < transformer.SrcRightBottom.Y;
            }
            public static bool ContainsBound(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
            {
                float a = (rightTop.X - leftTop.X) * (point.Y - leftTop.Y) - (rightTop.Y - leftTop.Y) * (point.X - leftTop.X);
                float b = (rightBottom.X - rightTop.X) * (point.Y - rightTop.Y) - (rightBottom.Y - rightTop.Y) * (point.X - rightTop.X);
                float c = (leftBottom.X - rightBottom.X) * (point.Y - rightBottom.Y) - (leftBottom.Y - rightBottom.Y) * (point.X - rightBottom.X);
                float d = (leftTop.X - leftBottom.X) * (point.Y - leftBottom.Y) - (leftTop.Y - leftBottom.Y) * (point.X - leftBottom.X);
                return (a > 0 && b > 0 && c > 0 && d > 0) || (a < 0 && b < 0 && c < 0 && d < 0);
            }



            /// <summary>
            /// Returns whether the radian area filled by the skew node contains the specified point. 
            /// </summary>
            /// <param name="point"> Input point. </param>
            /// <param name="transformer"> Layer's transformer. </param>
            /// <param name="matrix"></param>
            /// <returns></returns>
            public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 matrix)
            {
                //LTRB
                Vector2 leftTop = Vector2.Transform(transformer.DstLeftTop, matrix);
                Vector2 rightTop = Vector2.Transform(transformer.DstRightTop, matrix);
                Vector2 rightBottom = Vector2.Transform(transformer.DstRightBottom, matrix);
                Vector2 leftBottom = Vector2.Transform(transformer.DstLeftBottom, matrix);

                //Scale2
                if (Transformer.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
                if (Transformer.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
                if (Transformer.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
                if (Transformer.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

                //Center
                Vector2 centerLeft = (leftTop + leftBottom) / 2;
                Vector2 centerTop = (leftTop + rightTop) / 2;
                Vector2 centerRight = (rightTop + rightBottom) / 2;
                Vector2 centerBottom = (leftBottom + rightBottom) / 2;

                //Scale1
                if (Transformer.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
                if (Transformer.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
                if (Transformer.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
                if (Transformer.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Rotation
                if (transformer.DdisabledRadian == false)
                {
                    if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                    //Skew
                    //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                    //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
                    if (Transformer.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
                    if (Transformer.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;
                }

                //Translation
                if (Transformer.ContainsBound(point, leftTop, rightTop, rightBottom, leftBottom)) return TransformerMode.Translation;

                return TransformerMode.None;
            }


            #endregion


            #region Draw


            /// <summary> Draw lines on bound. </summary>
            public static void DrawBound(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
            {
                //LTRB
                Vector2 leftTop = Vector2.Transform(transformer.DstLeftTop, matrix);
                Vector2 rightTop = Vector2.Transform(transformer.DstRightTop, matrix);
                Vector2 rightBottom = Vector2.Transform(transformer.DstRightBottom, matrix);
                Vector2 leftBottom = Vector2.Transform(transformer.DstLeftBottom, matrix);

                //LTRB: Line
                ds.DrawLine(leftTop, rightTop, Windows.UI.Colors.DodgerBlue);
                ds.DrawLine(rightTop, rightBottom, Windows.UI.Colors.DodgerBlue);
                ds.DrawLine(rightBottom, leftBottom, Windows.UI.Colors.DodgerBlue);
                ds.DrawLine(leftBottom, leftTop, Windows.UI.Colors.DodgerBlue);
            }
            /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
            public static void DrawBoundNodes(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
            {
                //LTRB
                Vector2 leftTop = Vector2.Transform(transformer.DstLeftTop, matrix);
                Vector2 rightTop = Vector2.Transform(transformer.DstRightTop, matrix);
                Vector2 rightBottom = Vector2.Transform(transformer.DstRightBottom, matrix);
                Vector2 leftBottom = Vector2.Transform(transformer.DstLeftBottom, matrix);

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

                if (transformer.DdisabledRadian == false)
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

            /// <summary> Draw a —— </summary>
            public static void DrawLine(CanvasDrawingSession ds, Vector2 vector0, Vector2 vector1)
            {
                ds.DrawLine(vector0, vector1, Windows.UI.Colors.Black, 3);
                ds.DrawLine(vector0, vector1, Windows.UI.Colors.White);
            }


            #endregion


            #region Vector


            /// <summary> 15 degress in angle system. </summary>
            public const float RadiansStep = 0.2617993833333333f;
            /// <summary> 7.5 degress in angle system. </summary>
            public const float RadiansStepHalf = 0.1308996916666667f;
            /// <summary> To find a multiple of the nearest 15. </summary>
            public static float RadiansStepFrequency(float radian) => ((int)((radian + Transformer.RadiansStepHalf) / Transformer.RadiansStep)) * Transformer.RadiansStep;//Get step radians


            /// <summary> Math.PI </summary>
            public const float PI = 3.1415926535897931f;
            /// <summary> Half of Math.PI </summary>
            public const float PiHalf = 1.57079632679469655f;
            /// <summary> Quarter of Math.PI </summary>
            public const float PiQuarter = 0.78539816339734827f;


            /// <summary> Get the [Foot Point] of point and LIne. </summary>
            public static Vector2 FootPoint(Vector2 point, Vector2 lineA, Vector2 lineB)
            {
                Vector2 lineVector = lineA - lineB;
                Vector2 pointLineA = lineA - point;

                float t = -(pointLineA.Y * lineVector.Y + pointLineA.X * lineVector.X)
                    / lineVector.LengthSquared();

                return lineVector * t + lineA;
            }
            /// <summary> Get the  [Intersection Point] of Line1 and LIne2. </summary>
            public static Vector2 IntersectionPoint(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B)
            {
                float a = 0, b = 0;
                int state = 0;
                if (Math.Abs(line1A.X - line1B.X) > float.Epsilon)
                {
                    a = (line1B.Y - line1A.Y) / (line1B.X - line1A.X);
                    state |= 1;
                }
                if (Math.Abs(line2A.X - line2B.X) > float.Epsilon)
                {
                    b = (line2B.Y - line2A.Y) / (line2B.X - line2A.X);
                    state |= 2;
                }
                switch (state)
                {
                    case 0:
                        if (Math.Abs(line1A.X - line2A.X) < float.Epsilon) return Vector2.Zero;
                        else return Vector2.Zero;
                    case 1:
                        {
                            float x = line2A.X;
                            float y = (line1A.X - x) * (-a) + line1A.Y;
                            return new Vector2(x, y);
                        }
                    case 2:
                        {
                            float x = line1A.X;
                            float y = (line2A.X - x) * (-b) + line2A.Y;
                            return new Vector2(x, y);
                        }
                    case 3:
                        {
                            if (Math.Abs(a - b) < float.Epsilon) return Vector2.Zero;
                            float x = (a * line1A.X - b * line2A.X - line1A.Y + line2A.Y) / (a - b);
                            float y = a * x - a * line1A.X + line1A.Y;
                            return new Vector2(x, y);
                        }
                }
                return Vector2.Zero;
            }


            /// <summary>
            /// Get vector of the radians in the coordinate system. 
            /// </summary>
            /// <param name="radians">vector</param>
            /// <param name="center"> The center of coordinate system.  </param>
            /// <param name="length">The length of vector. </param>
            /// <returns></returns>
            public static Vector2 RadiansToVector(float radians, Vector2 center, float length = 40.0f)
            {
                return new Vector2((float)Math.Cos(radians) * length + center.X, (float)Math.Sin(radians) * length + center.Y);
            }
            /// <summary> Get radians of the vector in the coordinate system. </summary>
            public static float VectorToRadians(Vector2 vector)
            {
                float tan = (float)Math.Atan(Math.Abs(vector.Y / vector.X));

                //First Quantity
                if (vector.X > 0 && vector.Y > 0) return tan;
                //Second Quadrant
                else if (vector.X > 0 && vector.Y < 0) return -tan;
                //Third Quadrant  
                else if (vector.X < 0 && vector.Y > 0) return (float)Math.PI - tan;
                //Fourth Quadrant  
                else return tan - (float)Math.PI;
            }


            #endregion


        }

        //---------------------------------------------------------Controller--------------------------------------------------------//


        /// <summary> Controller interface </summary>
        public interface IController
        {
            void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix);
            void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix);
            void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix);
        }

        /// <summary>  </summary>
        public class Controller : IController
        {
            public TransformerMode Mode = TransformerMode.None;
            public readonly Dictionary<TransformerMode, IController> ControllerDictionary = new Dictionary<TransformerMode, IController>
            {
                {TransformerMode.None,  new NoneController()},
                {TransformerMode.Translation,  new TranslationController()},
                {TransformerMode.Rotation,  new RotationController()},

                {TransformerMode.SkewLeft,  new SkewLeftController()},
                {TransformerMode.SkewTop,  new SkewTopController()},
                {TransformerMode.SkewRight,  new SkewRightController()},
                {TransformerMode.SkewBottom,  new SkewBottomController()},

                {TransformerMode.ScaleLeft,  new ScaleLeftController()},
                {TransformerMode.ScaleTop,  new ScaleTopController()},
                {TransformerMode.ScaleRight,  new ScaleRightController()},
                {TransformerMode.ScaleBottom,  new ScaleBottomController()},

                {TransformerMode.ScaleLeftTop,  new ScaleLeftTopController()},
                {TransformerMode.ScaleRightTop,  new ScaleRightTopController()},
                {TransformerMode.ScaleRightBottom,  new ScaleRightBottomController()},
                {TransformerMode.ScaleLeftBottom,  new ScaleLeftBottomController()},
            };

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                this.Mode = Transformer.ContainsNodeMode(point, layer.Transformer, matrix);
                this.ControllerDictionary[this.Mode].Start(point, layer, matrix, inverseMatrix);
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                this.ControllerDictionary[this.Mode].Delta(point, layer, matrix, inverseMatrix);
            }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                this.ControllerDictionary[this.Mode].Delta(point, layer, matrix, inverseMatrix);
                this.Mode = TransformerMode.None;
            }
        }


        #region Controller


        /// <summary> None </summary>
        public class NoneController : IController
        {
            protected Transformer StartTransformer;

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix) => this.StartTransformer = layer.Transformer;
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix) { }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix) { }
        }

        /// <summary> Translation </summary>
        public class TranslationController : NoneController, IController
        {
            Vector2 StartPostion;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                base.Start(point, layer, matrix, inverseMatrix);
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);
                this.StartPostion = dstPoint;
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);
                Vector2 vector = dstPoint - this.StartPostion;

                Transformer.Add
                (
                    layer: layer,
                    startTransformer: base.StartTransformer,
                    vector: vector
                );
            }
        }

        /// <summary> Rotation </summary>
        public class RotationController : NoneController, IController
        {
            float StartRadian;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                base.Start(point, layer, matrix, inverseMatrix);
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);
                this.StartRadian = Transformer.VectorToRadians(dstPoint - base.StartTransformer.DstCenter);
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);
                float radian = -this.StartRadian + Transformer.VectorToRadians(dstPoint - base.StartTransformer.DstCenter);
                float radiansStepFrequency = HomographyController.IsStepFrequency ? Transformer.RadiansStepFrequency(radian) : radian;
                Matrix3x2 matrix2 = Matrix3x2.CreateRotation(radiansStepFrequency, base.StartTransformer.DstCenter);

                Transformer.Multiplies
                (
                    layer: layer,
                    startTransformer: base.StartTransformer,
                    matrix: matrix2
                );
            }
        }


        #endregion


        #region Skew


        /// <summary> Skew </summary>
        public abstract class SkewController : NoneController, IController
        {
            //@Override
            public abstract Vector2 GetLineA();
            public abstract Vector2 GetLineB();
            public abstract void SetTransformer(Layer layer, Vector2 vector);

            Vector2 StartSkew;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                base.Start(point, layer, matrix, inverseMatrix);

                this.StartSkew = Transformer.FootPoint(point, this.GetLineA(), this.GetLineB());
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                Vector2 postion = Transformer.FootPoint(point, this.GetLineA(), this.GetLineB());
                Vector2 vector = postion - this.StartSkew;
                this.SetTransformer(layer, vector);
            }
        }
        /// <summary> SkewLeft </summary>
        public class SkewLeftController : SkewController, IController
        {
            public override Vector2 GetLineA() => base.StartTransformer.DstLeftTop;
            public override Vector2 GetLineB() => base.StartTransformer.DstLeftBottom;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop + vector;
                layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop - vector;
                    layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom - vector;
                }
            }
        }
        /// <summary> SkewTop </summary>
        public class SkewTopController : SkewController, IController
        {
            public override Vector2 GetLineA() => base.StartTransformer.DstLeftTop;
            public override Vector2 GetLineB() => base.StartTransformer.DstRightTop;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop + vector;
                layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom - vector;
                    layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom - vector;
                }
            }
        }
        /// <summary> SkewRight </summary>
        public class SkewRightController : SkewController, IController
        {
            public override Vector2 GetLineA() => base.StartTransformer.DstRightTop;
            public override Vector2 GetLineB() => base.StartTransformer.DstRightBottom;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop + vector;
                layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop - vector;
                    layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom - vector;
                }
            }
        }
        /// <summary> SkewBottom </summary>
        public class SkewBottomController : SkewController, IController
        {
            public override Vector2 GetLineA() => base.StartTransformer.DstLeftBottom;
            public override Vector2 GetLineB() => base.StartTransformer.DstRightBottom;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom + vector;
                layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop - vector;
                    layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop - vector;
                }
            }
        }


        #endregion


        #region Scale


        /// <summary> 
        /// Distance of points on these points in a line: 
        /// ------D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
        /// </summary>
        public struct LineDistance
        {
            /// <summary> Distance between [Foot Point] and [Center Point] . </summary>
            public float FC;
            /// <summary> Distance between [Foot Point] and [Point] . </summary>
            public float FP;
            /// <summary> Distance between [Foot Point] and [Diagonal Point] . </summary>
            public float FD;
            /// <summary> Distance between [Point] and [Center Point] . </summary>
            public float PC;


            // These points in a line: 
            //      D[Diagonal Point]、C[Center Point]、P[Point] and F[FootPoint] .
            //
            //                                         2m                                           1m                          1m
            //————•————————————————•————————•————————•————
            //              D                                                          C                                                           P
            public LineDistance(Vector2 footPoint, Vector2 point, Vector2 center)
            {
                var diagonal = center + center - point;

                this.FC = Vector2.Distance(footPoint, center);
                this.FP = Vector2.Distance(footPoint, point);
                this.FD = Vector2.Distance(footPoint, diagonal);
                this.PC = Vector2.Distance(point, center);
            }

            /// <summary> Scale of [Foot Point] betwwen [Center Point] / scale of [Point] betwwen [Center Point] (may be negative)</summary>
            /// <param name="distance"> The distance </param>
            /// <returns>Scale</returns>
            public static float Scale(LineDistance distance)
            {
                float scale = distance.FC / distance.PC;
                bool isReverse = (distance.FP < distance.FD);
                return isReverse ? scale : -scale;
            }
        }


        /// <summary> Scale </summary>
        public abstract class ScaleController : NoneController, IController
        {
            //@Override
            public abstract Vector2 GetPoint();
            public abstract Vector2 GetDiagonalPoint();
        }


        #endregion


        #region ScaleAround


        /// <summary> ScaleAround (Left Top Right Bottom) </summary>
        public abstract class ScaleAroundController : ScaleController, IController
        {
            //@Override
            public abstract void SetTransformer(Layer layer, Vector2 vector);

            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);
                Vector2 footPoint = Transformer.FootPoint(dstPoint, this.GetPoint(), this.GetDiagonalPoint());

                if (HomographyController.IsRatio)
                {
                    Vector2 center = HomographyController.IsCenter ? base.StartTransformer.DstCenter : this.GetDiagonalPoint();

                    LineDistance distance = new LineDistance(footPoint, this.GetPoint(), center);
                    Matrix3x2 matrix2 = Matrix3x2.CreateScale(LineDistance.Scale(distance), center);

                    Transformer.Multiplies
                    (
                        layer: layer,
                        startTransformer: base.StartTransformer,
                        matrix: matrix2
                    );
                }
                else
                {
                    Vector2 vector = footPoint - this.GetPoint();

                    this.SetTransformer(layer, vector);
                }
            }
        }
        /// <summary> ScaleLeft </summary>
        public class ScaleLeftController : ScaleAroundController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstLeft;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstRight;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop + vector;
                layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop - vector;
                    layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom - vector;
                }
            }
        }
        /// <summary> ScaleTop </summary>
        public class ScaleTopController : ScaleAroundController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstTop;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstBottom;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop + vector;
                layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom - vector;
                    layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom - vector;
                }
            }
        }
        /// <summary> ScaleRight </summary>
        public class ScaleRightController : ScaleAroundController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstRight;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstLeft;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop + vector;
                layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop - vector;
                    layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom - vector;
                }
            }
        }
        /// <summary> ScaleBottom </summary>
        public class ScaleBottomController : ScaleAroundController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstBottom;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstTop;
            public override void SetTransformer(Layer layer, Vector2 vector)
            {
                layer.Transformer.DstLeftBottom = base.StartTransformer.DstLeftBottom + vector;
                layer.Transformer.DstRightBottom = base.StartTransformer.DstRightBottom + vector;

                if (HomographyController.IsCenter)
                {
                    layer.Transformer.DstLeftTop = base.StartTransformer.DstLeftTop - vector;
                    layer.Transformer.DstRightTop = base.StartTransformer.DstRightTop - vector;
                }
            }
        }


        #endregion


        #region ScaleCornerController


        /// <summary> ScaleCorner (LeftTop RightTop RightBottom LeftBottom) </summary>
        public abstract class ScaleCornerController : ScaleController, IController
        {
            //@Override
            public abstract void SetPoint(Layer layer, Vector2 point);
            public abstract void SetDiagonalPoint(Layer layer, Vector2 point);
            public abstract void SetHorizontalPoint(Layer layer, Vector2 point);
            public abstract void SetVerticalPoint(Layer layer, Vector2 point);

            protected Vector2 StartHorizontal;
            protected Vector2 StartVertical;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                base.Start(point, layer, matrix, inverseMatrix);
                this.StartHorizontal = layer.Transformer.DstRight - layer.Transformer.DstLeft;
                this.StartVertical = layer.Transformer.DstBottom - layer.Transformer.DstTop;
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, Matrix3x2 inverseMatrix)
            {
                Vector2 dstPoint = Vector2.Transform(point, inverseMatrix);

                if (HomographyController.IsRatio)
                {
                    Vector2 center = HomographyController.IsCenter ? base.StartTransformer.DstCenter : this.GetDiagonalPoint();
                    Vector2 footPoint = Transformer.FootPoint(dstPoint, this.GetPoint(), center);
                    LineDistance distance = new LineDistance(footPoint, this.GetPoint(), center);
                    Matrix3x2 matrix2 = Matrix3x2.CreateScale(LineDistance.Scale(distance), center);

                    Transformer.Multiplies
                    (
                        layer: layer,
                        startTransformer: base.StartTransformer,
                        matrix: matrix2
                    );
                }
                else
                {
                    Vector2 center = (HomographyController.IsCenter) ?
                        base.StartTransformer.DstCenter + base.StartTransformer.DstCenter - dstPoint :
                        this.GetDiagonalPoint();

                    this.SetPoint(layer, dstPoint);
                    this.SetDiagonalPoint(layer, center);
                    this.SetHorizontalPoint(layer, Transformer.IntersectionPoint
                    (
                       dstPoint,
                       dstPoint - this.StartHorizontal,
                       center + this.StartVertical,
                       center
                    ));
                    this.SetVerticalPoint(layer, Transformer.IntersectionPoint
                    (
                        line1A: dstPoint,
                        line1B: dstPoint - this.StartVertical,
                        line2A: center + this.StartHorizontal,
                        line2B: center
                    ));
                }
            }
        }
        /// <summary> ScaleLeftTop </summary>
        public class ScaleLeftTopController : ScaleCornerController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstLeftTop;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstRightBottom;
            public override void SetPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftTop = point;
            public override void SetDiagonalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightBottom = point;
            public override void SetHorizontalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightTop = point;
            public override void SetVerticalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftBottom = point;
        }
        /// <summary> ScaleRightTop </summary>
        public class ScaleRightTopController : ScaleCornerController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstRightTop;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstLeftBottom;
            public override void SetPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightTop = point;
            public override void SetDiagonalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftBottom = point;
            public override void SetHorizontalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftTop = point;
            public override void SetVerticalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightBottom = point;
        }
        /// <summary> ScaleRightBottom </summary>
        public class ScaleRightBottomController : ScaleCornerController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstRightBottom;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstLeftTop;
            public override void SetPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightBottom = point;
            public override void SetDiagonalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftTop = point;
            public override void SetHorizontalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftBottom = point;
            public override void SetVerticalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightTop = point;
        }
        /// <summary> ScaleLeftBottom </summary>
        public class ScaleLeftBottomController : ScaleCornerController
        {
            public override Vector2 GetPoint() => base.StartTransformer.DstLeftBottom;
            public override Vector2 GetDiagonalPoint() => base.StartTransformer.DstRightTop;
            public override void SetPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftBottom = point;
            public override void SetDiagonalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightTop = point;
            public override void SetHorizontalPoint(Layer layer, Vector2 point) => layer.Transformer.DstRightBottom = point;
            public override void SetVerticalPoint(Layer layer, Vector2 point) => layer.Transformer.DstLeftTop = point;
        }


        #endregion

    }
}