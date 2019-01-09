using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace Retouch_Photo.Models
{
    public struct Transformer
    {


        #region Matrix
        



        public float Width;
        public float Height;

        public float XScale;// = 1.0f;
        public float YScale;// = 1.0f;

        public Vector2 Postion;
        public float Radian;        
        public float Skew;

        public bool FlipHorizontal;
        public bool FlipVertical;
        public bool DisabledRadian;



        public Matrix3x2 Matrix => this.DisabledRadian ?

            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateScale(this.XScale, this.YScale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) *
            Matrix3x2.CreateTranslation(this.Postion) :

            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateScale(this.FlipHorizontal ? -this.XScale : this.XScale, this.FlipVertical ? -this.YScale : this.YScale) *
            Matrix3x2.CreateSkew(this.Skew, 0) *
            Matrix3x2.CreateRotation(this.Radian) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) *
            Matrix3x2.CreateTranslation(this.Postion);


        public Matrix3x2 InverseMatrix => this.DisabledRadian ?

            Matrix3x2.CreateTranslation(-this.Postion) *
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateScale(1 / this.XScale, 1 / this.YScale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) :

            Matrix3x2.CreateTranslation(-this.Postion) *
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateRotation(-this.Radian) *
            Matrix3x2.CreateSkew(-this.Skew, 0) *
            Matrix3x2.CreateScale(this.FlipHorizontal ? -1/this.XScale : 1 / this.XScale, this.FlipVertical ? -1 / this.YScale : 1 / this.YScale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);



        public void CopyWith(Transformer transformer)
        {
            this.XScale = transformer.XScale;
            this.YScale = transformer.YScale;

            this.Postion = transformer.Postion;
            this.Radian = transformer.Radian;
            this.Skew = transformer.Skew;

            this.FlipHorizontal = transformer.FlipHorizontal;
            this.FlipVertical = transformer.FlipVertical;
        }


        public static Transformer CreateFromRect(VectRect rect, float radian = 0.0f, bool disabledRadian = false) => new Transformer
        {
            Width = rect.Width,
            Height = rect.Height,

            XScale = 1.0f,
            YScale = 1.0f,

            Postion = new Vector2(rect.X, rect.Y),
            Radian = radian,
            Skew = 0,

            FlipHorizontal = false,
            FlipVertical = false,
            DisabledRadian = disabledRadian
        };
        public static Transformer CreateFromSize(float width, float height, float radian = 0.0f, bool disabledRadian = false) => new Transformer
        {
            Width = width,
            Height = height,

            XScale = 1.0f,
            YScale = 1.0f,

            Postion = Vector2.Zero,
            Radian = radian,
            Skew = 0,

            FlipHorizontal = false,
            FlipVertical = false,
            DisabledRadian = disabledRadian
        };


        #endregion


        #region Transform


        public Vector2 TransformLeft(Matrix3x2 matrix) => Vector2.Transform(new Vector2(0, this.Height / 2), matrix);
        public Vector2 TransformTop(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, 0), matrix);
        public Vector2 TransformRight(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, this.Height / 2), matrix);
        public Vector2 TransformBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, this.Height), matrix);

        public Vector2 TransformLeftTop(Matrix3x2 matrix) => matrix.Translation;
        public Vector2 TransformRightTop(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, 0), matrix);
        public Vector2 TransformRightBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, this.Height), matrix);
        public Vector2 TransformLeftBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(0, this.Height), matrix);

        public Vector2 TransformCenter(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, this.Height / 2), matrix);

        public float TransformMinX(Matrix3x2 matrix) => Math.Min(Math.Min(this.TransformLeftTop(Matrix).X, this.TransformRightTop(Matrix).X), Math.Min(this.TransformRightBottom(Matrix).X, this.TransformLeftBottom(Matrix).X));
        public float TransformMaxX(Matrix3x2 matrix) => Math.Max(Math.Max(this.TransformLeftTop(Matrix).X, this.TransformRightTop(Matrix).X), Math.Max(this.TransformRightBottom(Matrix).X, this.TransformLeftBottom(Matrix).X));
        public float TransformMinY(Matrix3x2 matrix) => Math.Min(Math.Min(this.TransformLeftTop(Matrix).Y, this.TransformRightTop(Matrix).Y), Math.Min(this.TransformRightBottom(Matrix).Y, this.TransformLeftBottom(Matrix).Y));
        public float TransformMaxY(Matrix3x2 matrix) => Math.Max(Math.Max(this.TransformLeftTop(Matrix).Y, this.TransformRightTop(Matrix).Y), Math.Max(this.TransformRightBottom(Matrix).Y, this.TransformLeftBottom(Matrix).Y));


        #endregion


        #region Contains


        /// <summary> Radius of node' . </summary>
        public static float NodeRadius = 12.0f;
        /// <summary> Whether the distance exceeds [NodeRadius].  </summary>
        public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;

        /// <summary> Minimum distance between two nodes. </summary>
        public static float NodeDistance = 20.0f;
        /// <summary> Double [NodeDistance]. </summary>
        public static float NodeDistanceDouble = 40.0f;
        /// <summary> Whether the distance exceeds [NodeDistance].  </summary>
        public static bool InNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;

        /// <summary> Returns whether the area filled by the bound rect contains the specified point. </summary>
        public static bool ContainsBound(Vector2 point, Transformer transformer)
        {
            Vector2 v = Vector2.Transform(point, transformer.InverseMatrix);
            return v.X > 0 && v.X < transformer.Width && v.Y > 0 && v.Y < transformer.Height;
        }


        #endregion


        #region Draw


        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBound(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop, Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Colors.DodgerBlue);
        }
        public static void DrawBoundNodesWithRotation(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            //LTRB
            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

            //LTRB: Line
            ds.DrawLine(leftTop, rightTop, Colors.DodgerBlue);
            ds.DrawLine(rightTop, rightBottom, Colors.DodgerBlue);
            ds.DrawLine(rightBottom, leftBottom, Colors.DodgerBlue);
            ds.DrawLine(leftBottom, leftTop, Colors.DodgerBlue);


            //Center: Vector2
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            float widthLength = (centerLeft - centerRight).Length();
            float heightLength = (centerTop - centerBottom).Length();

            //Center: Node
            if (widthLength > Transformer.NodeDistanceDouble)
            {
                Transformer.DrawNode2(ds, centerTop);
                Transformer.DrawNode2(ds, centerBottom);
            }
            if (heightLength > Transformer.NodeDistanceDouble)
            {
                Transformer.DrawNode2(ds, centerLeft);
                Transformer.DrawNode2(ds, centerRight);
            }

            if (transformer.DisabledRadian == false)
            {
                //Radian: Vector
                Vector2 radian = centerTop - Vector2.Normalize(centerBottom - centerTop) * Transformer.NodeDistanceDouble;
                //Radian: Line
                ds.DrawLine(radian, centerTop, Colors.DodgerBlue);
                //Radian: Node
                Transformer.DrawNode(ds, radian);
            }

            //LTRB: Node
            Transformer.DrawNode2(ds, leftTop);
            Transformer.DrawNode2(ds, rightTop);
            Transformer.DrawNode2(ds, rightBottom);
            Transformer.DrawNode2(ds, leftBottom);
        }
        public static void DrawBoundNodesWithSkew(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            //LTRB
            Vector2 centerLeft = transformer.TransformLeft(matrix);
            Vector2 centerTop = transformer.TransformTop(matrix);
            Vector2 centerRight = transformer.TransformRight(matrix);
            Vector2 centerBottom = transformer.TransformBottom(matrix);

            //Skew
            ds.DrawLine(centerLeft, centerRight, Colors.DodgerBlue);
            ds.DrawLine(centerTop, centerBottom, Colors.DodgerBlue);

            //LTRB: Node
            Transformer.DrawNode(ds, centerLeft);
            Transformer.DrawNode2(ds, centerTop);
            Transformer.DrawNode(ds, centerRight);
            Transformer.DrawNode(ds, centerBottom);
        }

        /// <summary> Draw a ⊙. </summary>
        public static void DrawNode(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Colors.White);
        }
        /// <summary> Draw a ●. </summary>
        public static void DrawNode2(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Colors.White);
            ds.FillCircle(vector, 6, Colors.DodgerBlue);
        }

        /// <summary> Draw a —— </summary>
        public static void DrawLine(CanvasDrawingSession ds, Vector2 vector0, Vector2 vector1)
        {
            ds.DrawLine(vector0, vector1, Colors.Black, 3);
            ds.DrawLine(vector0, vector1, Colors.White);
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
            /*
             * L1，L2都存在斜率的情况：
             * 直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 )
             * => y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
             * 令 a = ( y2 - y1 ) / ( x2 - x1 )
             * 有 y = a * x - a * x1 + y1   .........1
             * 直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
             * 令 b = ( y4 - y3 ) / ( x4 - x3 )
             * 有 y = b * x - b * x3 + y3 ..........2
             *
             * 如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
             * x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
             * y = a * x - a * x1 + y1
             *
             * L1存在斜率, L2平行Y轴的情况：
             * x = x3
             * y = a * x3 - a * x1 + y1
             *
             * L1 平行Y轴，L2存在斜率的情况：
             * x = x1
             * y = b * x - b * x3 + y3
             *
             * L1与L2都平行Y轴的情况：
             * 如果 x1 = x3，那么L1与L2重合，否则平等
             *
            */
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
                case 0: //L1与L2都平行Y轴
                    {
                        if (Math.Abs(line1A.X - line2A.X) < float.Epsilon)
                        {
                            //throw new Exception("两条直线互相重合，且平行于Y轴，无法计算交点。");
                            return Vector2.Zero;
                        }
                        else
                        {
                            //throw new Exception("两条直线互相平行，且平行于Y轴，无法计算交点。");
                            return Vector2.Zero;
                        }
                    }
                case 1: //L1存在斜率, L2平行Y轴
                    {
                        float x = line2A.X;
                        float y = (line1A.X - x) * (-a) + line1A.Y;
                        return new Vector2(x, y);
                    }
                case 2: //L1 平行Y轴，L2存在斜率
                    {
                        float x = line1A.X;
                        //网上有相似代码的，这一处是错误的。你可以对比case 1 的逻辑 进行分析
                        //源code:lineSecondStar * x + lineSecondStar * lineSecondStar.X + p3.Y;
                        float y = (line2A.X - x) * (-b) + line2A.Y;
                        return new Vector2(x, y);
                    }
                case 3: //L1，L2都存在斜率
                    {
                        if (Math.Abs(a - b) < float.Epsilon)
                        {
                            // throw new Exception("两条直线平行或重合，无法计算交点。");
                            return Vector2.Zero;
                        }
                        float x = (a * line1A.X - b * line2A.X - line1A.Y + line2A.Y) / (a - b);
                        float y = a * x - a * line1A.X + line1A.Y;
                        return new Vector2(x, y);
                    }
            }
            // throw new Exception("不可能发生的情况");
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


}
