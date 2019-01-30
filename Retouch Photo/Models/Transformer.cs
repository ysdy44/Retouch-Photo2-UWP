using Microsoft.Graphics.Canvas;
using Retouch_Photo.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using static Retouch_Photo.Library.TransformController;

namespace Retouch_Photo.Models
{
    /// <summary> Define Transformer. </summary>
    public struct Transformer222
    {


        #region Transformer


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
            Matrix3x2.CreateScale(this.FlipHorizontal ? -1 / this.XScale : 1 / this.XScale, this.FlipVertical ? -1 / this.YScale : 1 / this.YScale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);



        public void CopyWith(Transformer222 transformer)
        {
            this.XScale = transformer.XScale;
            this.YScale = transformer.YScale;

            this.Postion = transformer.Postion;
            this.Radian = transformer.Radian;
            this.Skew = transformer.Skew;

            this.FlipHorizontal = transformer.FlipHorizontal;
            this.FlipVertical = transformer.FlipVertical;
        }
        public static Transformer222 CreateFromSize(float width, float height,Vector2 postion, float scale = 1.0f, float radian = 0.0f, bool disabledRadian = false) => new Transformer222
        {
            Width = width,
            Height = height,

            XScale = scale,
            YScale = scale,

            Postion = postion,
            Radian = radian,
            Skew = 0,

            FlipHorizontal = false,
            FlipVertical = false,
            DisabledRadian = disabledRadian,
        };


        //Transform
        public Vector2 TransformLeft(Matrix3x2 matrix) => Vector2.Transform(new Vector2(0, this.Height / 2), matrix);
        public Vector2 TransformTop(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, 0), matrix);
        public Vector2 TransformRight(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, this.Height / 2), matrix);
        public Vector2 TransformBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, this.Height), matrix);

        public Vector2 TransformLeftTop(Matrix3x2 matrix) => matrix.Translation;
        public Vector2 TransformRightTop(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, 0), matrix);
        public Vector2 TransformRightBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, this.Height), matrix);
        public Vector2 TransformLeftBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(0, this.Height), matrix);

        public Vector2 TransformCenter(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width / 2, this.Height / 2), matrix);


        //Operate
        public float TransformMinX(Matrix3x2 matrix) => Math.Min(Math.Min(this.TransformLeftTop(Matrix).X, this.TransformRightTop(Matrix).X), Math.Min(this.TransformRightBottom(Matrix).X, this.TransformLeftBottom(Matrix).X));
        public float TransformMaxX(Matrix3x2 matrix) => Math.Max(Math.Max(this.TransformLeftTop(Matrix).X, this.TransformRightTop(Matrix).X), Math.Max(this.TransformRightBottom(Matrix).X, this.TransformLeftBottom(Matrix).X));
        public float TransformMinY(Matrix3x2 matrix) => Math.Min(Math.Min(this.TransformLeftTop(Matrix).Y, this.TransformRightTop(Matrix).Y), Math.Min(this.TransformRightBottom(Matrix).Y, this.TransformLeftBottom(Matrix).Y));
        public float TransformMaxY(Matrix3x2 matrix) => Math.Max(Math.Max(this.TransformLeftTop(Matrix).Y, this.TransformRightTop(Matrix).Y), Math.Max(this.TransformRightBottom(Matrix).Y, this.TransformLeftBottom(Matrix).Y));


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
        /// <summary> Whether the distance exceeds [NodeDistance]. Defult: 400. </summary>
        public static bool OutNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;


        /// <summary> Get outside node. </summary>
        public static Vector2 OutsideNode(Vector2 nearNode, Vector2 farNode) => nearNode - Vector2.Normalize(farNode - nearNode) * Transformer222.NodeDistanceDouble;


        /// <summary> Returns whether the area filled by the bound rect contains the specified point. </summary>
        public static bool ContainsBound(Vector2 point, Transformer222 transformer)
        {
            Vector2 v = Vector2.Transform(point, transformer.InverseMatrix);
            return v.X > 0 && v.X < transformer.Width && v.Y > 0 && v.Y < transformer.Height;
        }
                       

        /// <summary>
        /// Returns whether the radian area filled by the skew node contains the specified point. 
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> Layer's transformer. </param>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer222 transformer, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

            //Scale2
            if (Transformer222.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
            if (Transformer222.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
            if (Transformer222.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
            if (Transformer222.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale1
            if (Transformer222.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
            if (Transformer222.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
            if (Transformer222.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
            if (Transformer222.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

            //Outside
            Vector2 outsideLeft = Transformer222.OutsideNode(centerLeft, centerRight);
            Vector2 outsideTop = Transformer222.OutsideNode(centerTop, centerBottom);
            Vector2 outsideRight = Transformer222.OutsideNode(centerRight, centerLeft);
            Vector2 outsideBottom = Transformer222.OutsideNode(centerBottom, centerTop);

            //Rotation
            if (Transformer222.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

            //Rotation
            //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
            //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
            if (Transformer222.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
            if (Transformer222.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;

            //Translation
            if (Transformer222.ContainsBound(point, transformer)) return TransformerMode.Translation;

            return TransformerMode.None;
        }

        #endregion


        #region Draw


        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBound(CanvasDrawingSession ds, Transformer222 transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix =canvasToVirtualToControlMatrix;

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
        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBoundNodes(CanvasDrawingSession ds, Transformer222 transformer, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

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
            Transformer222.DrawNode2(ds, leftTop);
            Transformer222.DrawNode2(ds, rightTop);
            Transformer222.DrawNode2(ds, rightBottom);
            Transformer222.DrawNode2(ds, leftBottom);

            //Scale1
            if (Transformer222.OutNodeDistance(centerLeft, centerRight))
            {
                Transformer222.DrawNode2(ds, centerTop);
                Transformer222.DrawNode2(ds, centerBottom);
            }
            if (Transformer222.OutNodeDistance(centerTop, centerBottom))
            {
                Transformer222.DrawNode2(ds, centerLeft);
                Transformer222.DrawNode2(ds, centerRight);
            }

            //Outside
            Vector2 outsideLeft = Transformer222.OutsideNode(centerLeft, centerRight);
            Vector2 outsideTop = Transformer222.OutsideNode(centerTop, centerBottom);
            Vector2 outsideRight = Transformer222.OutsideNode(centerRight, centerLeft);
            Vector2 outsideBottom = Transformer222.OutsideNode(centerBottom, centerTop);

            //Radian
            ds.DrawLine(outsideTop, centerTop, Windows.UI.Colors.DodgerBlue);
            Transformer222.DrawNode(ds, outsideTop);

            //Skew:
            //Transformer.DrawNode2(ds, outsideLeft);
            //Transformer.DrawNode2(ds, outsideTop);
            Transformer222.DrawNode2(ds, outsideRight);
            Transformer222.DrawNode2(ds, outsideBottom);
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
        public static float RadiansStepFrequency(float radian) => ((int)((radian + Transformer222.RadiansStepHalf) / Transformer222.RadiansStep)) * Transformer222.RadiansStep;//Get step radians


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


}
