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

        #region Transformer & Matrix


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



        public Matrix3x2 Matrix => this.DisabledRadian ? Matrix3x2.CreateTranslation(this.Postion) :
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateScale(this.FlipHorizontal ? -this.XScale : this.XScale, this.FlipVertical ? -this.YScale : this.YScale) *
            Matrix3x2.CreateSkew(this.Skew, 0) *
            Matrix3x2.CreateRotation(this.Radian) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) *
            Matrix3x2.CreateTranslation(this.Postion);

        public Matrix3x2 InverseMatrix => this.DisabledRadian ? Matrix3x2.CreateTranslation(-this.Postion) :
            Matrix3x2.CreateTranslation(-this.Postion) *
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
            Matrix3x2.CreateRotation(-this.Radian) *
            Matrix3x2.CreateSkew(-this.Skew, 0) *
            Matrix3x2.CreateScale(this.FlipHorizontal ? -this.XScale : this.XScale, this.FlipVertical ? -this.YScale : this.YScale) *
            Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);




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


        #region Contains & Transform


        /// <summary> Radius of node' . </summary>
        public static float NodeRadius = 12.0f;
        public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;

        /// <summary> Minimum distance between two nodes. </summary>
        public static float NodeDistance = 20.0f;
        public static float NodeDistanceDouble = 40.0f;
        public static bool InNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;

         

        /// <summary> Returns whether the area filled by the bound rect contains the specified point. </summary>
        public static bool ContainsBound(Vector2 point, Transformer transformer)
        {
            Vector2 v = Vector2.Transform(point, transformer.InverseMatrix);
            return v.X > 0 && v.X < transformer.Width && v.Y > 0 && v.Y < transformer.Height;
        }

        public Vector2 TransformLeftTop(Matrix3x2 matrix) => matrix.Translation;
        public Vector2 TransformRightTop(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, 0), matrix);
        public Vector2 TransformRightBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width, this.Height), matrix);
        public Vector2 TransformLeftBottom(Matrix3x2 matrix) => Vector2.Transform(new Vector2(0, this.Height), matrix);
        public Vector2 TransformCenter(Matrix3x2 matrix) => Vector2.Transform(new Vector2(this.Width/2, this.Height/2), matrix);

        /// <summary> Returns whether the radian area filled by the skew node contains the specified point. </summary>
        public static CursorMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix, bool isCtrl = false)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            //LTRB
            Vector2 leftTop = transformer.TransformLeftTop(matrix);
            Vector2 rightTop = transformer.TransformRightTop(matrix);
            Vector2 rightBottom = transformer.TransformRightBottom(matrix);
            Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            if (isCtrl == false)
            {
                //Scale
                if (Transformer.InNodeRadius(leftTop, point)) return CursorMode.ScaleLeftTop;
                if (Transformer.InNodeRadius(rightTop, point)) return CursorMode.ScaleRightTop;
                if (Transformer.InNodeRadius(rightBottom, point)) return CursorMode.ScaleRightBottom;
                if (Transformer.InNodeRadius(leftBottom, point)) return CursorMode.ScaleLeftBottom;

                //Scale
                if (Transformer.InNodeRadius(centerLeft, point)) return CursorMode.ScaleLeft;
                if (Transformer.InNodeRadius(centerTop, point)) return CursorMode.ScaleTop;
                if (Transformer.InNodeRadius(centerRight, point)) return CursorMode.ScaleRight;
                if (Transformer.InNodeRadius(centerBottom, point)) return CursorMode.ScaleBottom;
            }

            if (isCtrl == false && transformer.DisabledRadian == false)
            {
                //Rotation
                Vector2 radians = centerTop - Vector2.Normalize(centerBottom - centerTop) * Transformer.NodeDistanceDouble;
                if (Transformer.InNodeRadius(radians, point)) return CursorMode.Rotation;
            }

            if (isCtrl && transformer.DisabledRadian == false)
            {
                //Skew
                if (Transformer.InNodeRadius(centerLeft, point)) return CursorMode.SkewLeft;
                if (Transformer.InNodeRadius(centerTop, point)) return CursorMode.SkewTop;
                if (Transformer.InNodeRadius(centerRight, point)) return CursorMode.SkewRight;
                if (Transformer.InNodeRadius(centerBottom, point)) return CursorMode.SkewBottom;
            }
            
            return CursorMode.None;
        }


        #endregion


        #region Draw


        /// <summary> Draw nodes and lines on bound，just like【由】. </summary>
        public static void DrawBound(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 canvasToVirtualToControlMatrix)
        {
            Matrix3x2 matrix = transformer.Matrix * canvasToVirtualToControlMatrix;

            Vector2 leftTop = matrix.Translation;
            Vector2 rightTop = Vector2.Transform(new Vector2(transformer.Width, 0), matrix);
            Vector2 rightBottom = Vector2.Transform(new Vector2(transformer.Width, transformer.Height), matrix);
            Vector2 leftBottom = Vector2.Transform(new Vector2(0, transformer.Height), matrix);

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
            Vector2 leftTop = matrix.Translation;
            Vector2 rightTop = Vector2.Transform(new Vector2(transformer.Width, 0), matrix);
            Vector2 rightBottom = Vector2.Transform(new Vector2(transformer.Width, transformer.Height), matrix);
            Vector2 leftBottom = Vector2.Transform(new Vector2(0, transformer.Height), matrix);

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
            Vector2 centerLeft = Vector2.Transform(new Vector2(0, transformer.Height / 2), matrix);
            Vector2 centerTop = Vector2.Transform(new Vector2(transformer.Width / 2, 0), matrix);
            Vector2 centerRight = Vector2.Transform(new Vector2(transformer.Width, transformer.Height / 2), matrix);
            Vector2 centerBottom = Vector2.Transform(new Vector2(transformer.Width / 2, transformer.Height), matrix);

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


        #region Vector2


        public static readonly float RadiansStep = 0.2617993833333333f;//15 degress in angle system
        public static readonly float RadiansStepHalf = 0.1308996916666667f;//7.5 degress in angle system
        public static float RadiansStepFrequency(float radian) => ((int)((radian + Transformer.RadiansStepHalf) / Transformer.RadiansStep)) * Transformer.RadiansStep;//Get step radians


        public static readonly float PiHalf = 1.57079632679469655f;//Half of Math.PI
        public static readonly float PiQuarter = 0.78539816339734827f;//Half of Math.PI
        
        public static Vector2 FootPoint(Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            Vector2 lineVector = lineA - lineB;
            Vector2 pointA = lineA - point;

            float t = -(pointA.Y * lineVector.Y + pointA.X * lineVector.X)
                / lineVector.LengthSquared();

            return lineVector * t + lineA;
        }


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
        public static Vector2 RadiansToVector(float radians, Vector2 center, float distance = 40.0f)
        {
            return new Vector2((float)Math.Cos(radians) * distance + center.X, (float)Math.Sin(radians) * distance + center.Y);
        }


        #endregion

    }


    public enum CursorMode
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


}
