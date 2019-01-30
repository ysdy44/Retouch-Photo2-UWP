using Microsoft.Graphics.Canvas;
using Retouch_Photo.Models;
using Retouch_Photo.ViewModels;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo.Library
{
    /// <summary> Drag and drop nodes and manipulate transformer libraries. </summary>
    public class TransformController
    {

        /// <summary> Scaling around the center. </summary>
        public static bool IsCenter => (App.ViewModel.MarqueeMode == MarqueeMode.Center) || (App.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);
        /// <summary> Maintain a ratio when scaling. </summary>
        public static bool IsRatio => (App.ViewModel.MarqueeMode == MarqueeMode.Square) || (App.ViewModel.MarqueeMode == MarqueeMode.SquareAndCenter);
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


            public float Width;
            public float Height;

            public float XScale;// = 1.0f;
            public float YScale;// = 1.0f;

            public Vector2 Postion;
            public float Radian;
            public float Skew;
            public bool DisabledRadian;


            public Matrix3x2 Matrix => this.DisabledRadian ?
                Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
                Matrix3x2.CreateScale(this.XScale, this.YScale) *
                Matrix3x2.CreateTranslation(this.Postion) :
            Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) *
                Matrix3x2.CreateScale(this.XScale, this.YScale) *
                Matrix3x2.CreateSkew(this.Skew, 0) *
                Matrix3x2.CreateRotation(this.Radian) *
                Matrix3x2.CreateTranslation(this.Postion);


            public Matrix3x2 InverseMatrix => this.DisabledRadian ?
                Matrix3x2.CreateTranslation(-this.Postion) *
                Matrix3x2.CreateScale(1 / this.XScale, 1 / this.YScale) *
                Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2) :
            Matrix3x2.CreateTranslation(-this.Postion) *
                Matrix3x2.CreateRotation(-this.Radian) *
                Matrix3x2.CreateSkew(-this.Skew, 0) *
                Matrix3x2.CreateScale(1 / this.XScale, 1 / this.YScale) *
                Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);


            public void CopyWith(Transformer transformer)
            {
                this.XScale = transformer.XScale;
                this.YScale = transformer.YScale;

                this.Postion = transformer.Postion;
                this.Radian = transformer.Radian;
                this.Skew = transformer.Skew;
            }
            public static Transformer CreateFromSize(float width, float height, Vector2 postion, float scale = 1.0f, float radian = 0.0f, bool disabledRadian = false) => new Transformer
            {
                Width = width,
                Height = height,

                XScale = scale,
                YScale = scale,

                Postion = postion,
                Radian = radian,
                Skew = 0,
                DisabledRadian = disabledRadian
            };

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
                return v.X > 0 && v.X < transformer.Width && v.Y > 0 && v.Y < transformer.Height;
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
                Vector2 leftTop = transformer.TransformLeftTop(matrix);
                Vector2 rightTop = transformer.TransformRightTop(matrix);
                Vector2 rightBottom = transformer.TransformRightBottom(matrix);
                Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

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
                if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                //Rotation
                //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
                if (Transformer.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
                if (Transformer.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;

                //Translation
                if (Transformer.ContainsBound(point, transformer)) return TransformerMode.Translation;

                return TransformerMode.None;
            }

            #endregion


            #region Draw


            /// <summary> Draw lines on bound. </summary>
            public static void DrawBound(CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix)
            {
                Vector2 leftTop = transformer.TransformLeftTop(matrix);
                Vector2 rightTop = transformer.TransformRightTop(matrix);
                Vector2 rightBottom = transformer.TransformRightBottom(matrix);
                Vector2 leftBottom = transformer.TransformLeftBottom(matrix);

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

                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Radian
                ds.DrawLine(outsideTop, centerTop, Windows.UI.Colors.DodgerBlue);
                Transformer.DrawNode(ds, outsideTop);

                //Skew:
                //Transformer.DrawNode2(ds, outsideLeft);
                //Transformer.DrawNode2(ds, outsideTop);
                Transformer.DrawNode2(ds, outsideRight);
                Transformer.DrawNode2(ds, outsideBottom);
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
            void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale);
            void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale);
            void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale);
        }

        /// <summary> Controller </summary>
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

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.Mode = Transformer.ContainsNodeMode(point, layer.Transformer, layer.Transformer.Matrix);
                this.ControllerDictionary[this.Mode].Start(point, layer, matrix, scale);
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.ControllerDictionary[this.Mode].Delta(point, layer, matrix, scale);
            }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.ControllerDictionary[this.Mode].Delta(point, layer, matrix, scale);
                this.Mode = TransformerMode.None;
            }
        }


        #region Controller


        /// <summary> None </summary>
        private class NoneController : IController
        {
            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
        }

        /// <summary> Translation </summary>
        private class TranslationController : IController
        {
            Vector2 StartTransformerPostion;
            Vector2 StartPostion;

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                Vector2 vector = this.GetTransformVector(point, layer, matrix);

                this.StartPostion = vector;
                this.StartTransformerPostion = layer.Transformer.Postion;
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                Vector2 vector = this.GetTransformVector(point, layer, matrix);

                layer.Transformer.Postion = vector + (this.StartTransformerPostion - this.StartPostion);
            }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }

            /// <summary>
            /// Get the transformed vector
            /// </summary>
            /// <param name="transformMatrix"> Transform matrix of Canvas (If there is).</param>
            /// <returns></returns>
            private Vector2 GetTransformVector(Vector2 point, Layer layer, Matrix3x2 transformMatrix)
            {
                Matrix3x2 matrix = layer.Transformer.Matrix;

                if (matrix == transformMatrix) return point;

                return new Vector2(point.X * matrix.M11 / matrix.M11, point.Y * matrix.M22 / matrix.M22);
            }
        }

        /// <summary> Rotation </summary>
        private class RotationController : IController
        {
            Vector2 Center;
            float Radian;

            float StartTransformerRadian;
            float StartRadian;

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.Center = layer.Transformer.TransformCenter(matrix);

                this.StartTransformerRadian = layer.Transformer.Radian;
                this.StartRadian = Transformer.VectorToRadians(point - this.Center);
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.Radian = Transformer.VectorToRadians(point - this.Center);

                float radian = this.StartTransformerRadian - this.StartRadian + this.Radian;

                layer.Transformer.Radian = TransformController.IsStepFrequency ? Transformer.RadiansStepFrequency(radian) : radian;
            }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
        }


        #endregion


        #region Skew


        /// <summary> Skew </summary>
        protected abstract class SkewController : IController
        {
            //@Override
            public abstract Vector2 GetLineA(Layer layer, Matrix3x2 matrix);
            public abstract Vector2 GetLineB(Layer layer, Matrix3x2 matrix);

            protected Transformer StartTransformer;
            protected Vector2 Center;

            /// <summary> Point A (left point on the same side of the point) </summary>
            protected Vector2 LineA;
            /// <summary> Point b (right point on the same side of the point) </summary>
            protected Vector2 LineB;

            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.StartTransformer.CopyWith(layer.Transformer);

                //Center
                this.Center = layer.Transformer.TransformCenter(matrix);

                this.LineA = this.GetLineA(layer, matrix);
                this.LineB = this.GetLineB(layer, matrix);
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
        }


        /// <summary> SkewVertical (Top Bottom) </summary>
        protected abstract class SkewVerticalController : SkewController, IController
        {
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                Vector2 footPoint = Transformer.FootPoint(point, base.LineA, base.LineB);
                float radians = Transformer.VectorToRadians(footPoint - this.Center);

                float value = -radians + this.StartTransformer.Radian + Transformer.PiHalf;
                layer.Transformer.Skew = value;
            }
        }
        private class SkewTopController : SkewVerticalController
        {
            public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
            public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
        }
        private class SkewBottomController : SkewVerticalController
        {
            public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
            public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
        }


        /// <summary> SkewHorizontal (Left Right)</summary>
        protected abstract class SkewHorizontalController : SkewController, IController
        {
            //@Override
            public abstract void SetRadian(Layer layer, float skew);

            /// <summary> Visually really Scale (For Skew, Scale is't really Scale.) </summary>
            Vector2 RealScale;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                base.Start(point, layer, matrix, scale);

                //RealScale
                float value = this.StartTransformer.Skew;
                float cos = (float)Math.Abs(Math.Cos(value));
                this.RealScale = new Vector2(this.StartTransformer.XScale * cos, this.StartTransformer.YScale / cos);
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                Vector2 footPoint = Transformer.FootPoint(point, base.LineA, base.LineB);
                float radians = Transformer.VectorToRadians(footPoint - this.Center);

                //Radian
                this.SetRadian(layer, radians);

                //Skew
                float value = radians + (this.StartTransformer.Skew - this.StartTransformer.Radian);
                layer.Transformer.Skew = value;

                //Scale
                float cos = (float)Math.Abs(Math.Cos(value));
                layer.Transformer.XScale = this.RealScale.X / cos;
                layer.Transformer.YScale = this.RealScale.Y * cos;
            }
        }
        private class SkewLeftController : SkewHorizontalController
        {
            public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
            public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);

            public override void SetRadian(Layer layer, float skew) => layer.Transformer.Radian = skew + Transformer.PI;
        }
        private class SkewRightController : SkewHorizontalController
        {
            public override Vector2 GetLineA(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
            public override Vector2 GetLineB(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);

            public override void SetRadian(Layer layer, float skew) => layer.Transformer.Radian = skew;
        }


        #endregion


        #region Scale


        /// <summary>
        /// These points in a line:  
        /// ------S[Symmetric Point]、D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
        /// </summary>
        public struct VectorLine
        {
            /// <summary> Diagonal Point(Such as: Left Point)</summary>
            public Vector2 Diagonal;

            /// <summary> Symmetric Point(Diagonal Point as origin, the Symmetric Point of a Point) </summary>
            public Vector2 Symmetric;

            /// <summary> Center Point (The Center Point between Point and Diagonal Point) </summary>
            public Vector2 Center;

            // These points in a line: 
            //      S[Symmetric Point]、D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
            //
            //                                         2m                                           1m                          1m
            //————•————————————————•————————•————————•————
            //              S                                                          D                            C                             P
        }

        /// <summary> Distance of points on the [VectorLine]. </summary>
        public struct VectorDistance
        {
            /// <summary> Distance between [Foot Point] and [Diagonal Point] . </summary>
            public float FD;
            /// <summary> Distance between [Foot Point] and [Point] . </summary>
            public float FP;
            /// <summary> Distance between [Foot Point] and [Center Point] . </summary>
            public float FC;
            /// <summary> Distance between [Point] and [Center Point] .</summary>
            public float PC;
            /// <summary> Distance between [Foot Point] and [Symmetric Point] . </summary>
            public float FS;
            /// <summary> Distance between [Point] and [Diagonal Point] . </summary>
            public float PD;
        }

        /// <summary> It has four float.:  XCos, XSin, YCos, YSin; </summary>
        public struct VectorSinCos
        {
            /// <summary> The XCos component of the VectorSinCos. </summary>
            public float XCos;
            /// <summary> The XSin component of the VectorSinCos. </summary>
            public float XSin;
            /// <summary> The YCos component of the VectorSinCos. </summary>
            public float YCos;
            /// <summary> The YSin component of the VectorSinCos. </summary>
            public float YSin;

            /// <summary>
            /// Creates a vector whose elements have the specified values.
            /// </summary>
            /// <param name="xCos"></param>
            /// <param name="xSin"></param>
            /// <param name="yCos"></param>
            /// <param name="ySin"></param>
            public VectorSinCos(float xCos, float xSin, float yCos, float ySin)
            {
                this.XCos = xCos;
                this.XSin = xSin;
                this.YCos = yCos;
                this.YSin = ySin;
            }




            public static VectorSinCos Reverse(VectorSinCos value, bool isReverse)
            {
                if (isReverse) return value;

                return new VectorSinCos(-value.XCos, -value.XSin, -value.YCos, -value.YSin);
            }
            public static VectorSinCos Reverse(VectorSinCos value, bool isXReverse, bool isYReverse)
            {
                if (!isXReverse && !isYReverse) return value;

                if (!isXReverse && isYReverse) return new VectorSinCos(value.XCos, value.XSin, -value.YCos, -value.YSin);

                if (isXReverse && !isYReverse) return new VectorSinCos(-value.XCos, -value.XSin, value.YCos, value.YSin);

                return VectorSinCos.Reverse(value, false);
            }
        }

        /// <summary> Scale </summary>         
        public abstract class ScaleController : IController
        {
            //@Override
            public abstract Vector2 GetPoint(Layer layer, Matrix3x2 matrix);
            public abstract Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix);
            public abstract void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale);


            protected Transformer StartTransformer;

            protected Vector2 Point;
            protected VectorLine Line;

            protected VectorSinCos SinCos;


            public void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                this.StartTransformer.CopyWith(layer.Transformer);

                float x = -layer.Transformer.Radian;
                this.SinCos.XCos = (float)Math.Cos(x);
                this.SinCos.XSin = (float)Math.Sin(x);

                float y = -layer.Transformer.Radian + layer.Transformer.Skew;
                this.SinCos.YCos = (float)Math.Cos(y);
                this.SinCos.YSin = (float)Math.Sin(y);
            }
            public void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }
            public void Complete(Vector2 point, Layer layer, Matrix3x2 matrix, float scale) { }

            public VectorDistance GetVectorDistance(Vector2 footPoint, Vector2 point, VectorLine line) => new VectorDistance
            {
                FD = Vector2.Distance(footPoint, line.Diagonal),
                FP = Vector2.Distance(footPoint, point),
                FC = Vector2.Distance(footPoint, line.Center),
                PC = Vector2.Distance(point, line.Center),
                FS = Vector2.Distance(footPoint, line.Symmetric),
                PD = Vector2.Distance(point, line.Diagonal),
            };
        }


        #endregion


        #region ScaleAround


        /// <summary> ScaleAround (Left Top Right Bottom) </summary>
        public abstract class ScaleAroundController : ScaleController, IController
        {
            bool IsCenter => TransformController.IsCenter;
            bool IsRatio => TransformController.IsRatio;

            //@Override
            public abstract void SetScale(Layer layer, float scale, bool isRatio);
            public abstract void SetFlip(Layer layer, bool isFlip);


            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                base.Start(point, layer, matrix, scale);
                base.Point = this.GetPoint(layer, matrix);

                //Diagonal line
                Vector2 diagonal = this.GetDiagonal(layer, matrix);
                base.Line = new VectorLine
                {
                    Diagonal = diagonal,
                    Symmetric = diagonal + diagonal - base.Point,
                    Center = layer.Transformer.TransformCenter(matrix)
                };
            }
            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                //Point on diagonal line
                Vector2 footPoint = Transformer.FootPoint(point, base.Line.Diagonal, base.Point);
                VectorDistance distance = base.GetVectorDistance(footPoint, base.Point, this.Line);

                //Scale with Center
                if (this.IsCenter)
                {
                    //Scale
                    float scale1 = distance.FC / distance.PC;
                    this.SetScale(layer, scale1, this.IsRatio);

                    //Flip
                    bool isFlip = distance.FD > distance.FP;
                    this.SetFlip(layer, isFlip);
                }

                //Scale with Side
                else
                {
                    //Scale
                    float scale1 = distance.FD / distance.PD;
                    this.SetScale(layer, scale1, this.IsRatio);

                    //Flip
                    bool isFlip = distance.FS > distance.FP;
                    this.SetFlip(layer, isFlip);

                    //Postion
                    float move = distance.FP / 2;
                    this.SetReversePostion(layer, distance, new VectorSinCos
                    (
                            xCos: base.SinCos.XCos * move,
                            xSin: base.SinCos.XSin * move,

                            yCos: base.SinCos.YCos * move,
                            ySin: base.SinCos.YSin * move
                    ), scale);
                }
            }


            //Postion: Reverse
            public void SetReversePostion(Layer layer, VectorDistance distance, VectorSinCos sinCos, float scale)
            {
                bool isReverse = distance.FD < distance.PD ? true : distance.FD < distance.FP;//F in the left of the P ?

                this.SetPostion(layer, this.StartTransformer, VectorSinCos.Reverse(sinCos, isReverse), scale);
            }
        }
        public class ScaleLeftController : ScaleAroundController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeft(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRight(matrix);

            public override void SetScale(Layer layer, float scale, bool isRatio)
            {
                layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                if (isRatio) layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
            public override void SetFlip(Layer layer, bool isFlip)
            {
                bool flip = (this.StartTransformer.XScale < 0) == isFlip;
                layer.Transformer.XScale = flip ? -layer.Transformer.XScale : layer.Transformer.XScale;
            }
            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + sinCos.XCos / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y - sinCos.XSin / scale;
            }
        }
        public class ScaleTopController : ScaleAroundController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformTop(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformBottom(matrix);

            public override void SetScale(Layer layer, float scale, bool isRatio)
            {
                if (isRatio) layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
            public override void SetFlip(Layer layer, bool isFlip)
            {
                bool flip = (this.StartTransformer.YScale < 0) == isFlip;
                layer.Transformer.YScale = flip ? -layer.Transformer.YScale : layer.Transformer.YScale;
            }
            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + sinCos.YSin / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + sinCos.YCos / scale;
            }
        }
        public class ScaleRightController : ScaleAroundController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRight(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeft(matrix);

            public override void SetScale(Layer layer, float scale, bool isRatio)
            {
                layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                if (isRatio) layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
            public override void SetFlip(Layer layer, bool isFlip)
            {
                bool flip = (this.StartTransformer.XScale < 0) == isFlip;
                layer.Transformer.XScale = flip ? -layer.Transformer.XScale : layer.Transformer.XScale;
            }
            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - sinCos.XCos / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + sinCos.XSin / scale;
            }
        }
        public class ScaleBottomController : ScaleAroundController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformBottom(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformTop(matrix);

            public override void SetScale(Layer layer, float scale, bool isRatio)
            {
                if (isRatio) layer.Transformer.XScale = this.StartTransformer.XScale * scale;
                layer.Transformer.YScale = this.StartTransformer.YScale * scale;
            }
            public override void SetFlip(Layer layer, bool isFlip)
            {
                bool flip = (this.StartTransformer.YScale < 0) == isFlip;
                layer.Transformer.YScale = flip ? -layer.Transformer.YScale : layer.Transformer.YScale;
            }
            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X - sinCos.YSin / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y - sinCos.YCos / scale;
            }
        }


        #endregion


        #region ScaleCorner


        /// <summary> ScaleCorner (LeftTop RightTop RightBottom LeftBottom)</summary>
        public abstract class ScaleCornerController : ScaleController, IController
        {
            bool IsCenter => TransformController.IsCenter;
            bool IsRatio => TransformController.IsRatio;

            //@Override
            public abstract Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix);
            public abstract Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix);

            VectorLine HorizontalLine;
            VectorLine VerticalLine;

            public new void Start(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                base.Start(point, layer, matrix, scale);
                base.Point = this.GetPoint(layer, matrix);

                //Diagonal line
                Vector2 diagonal = this.GetDiagonal(layer, matrix);
                this.Line = new VectorLine
                {
                    Diagonal = diagonal,
                    Symmetric = diagonal + diagonal - base.Point,
                    Center = layer.Transformer.TransformCenter(matrix)
                };

                //Horizontal line
                Vector2 horizontalDiagonal = this.GetHorizontalDiagonal(layer, matrix);
                this.HorizontalLine = new VectorLine
                {
                    Diagonal = horizontalDiagonal,
                    Symmetric = horizontalDiagonal + horizontalDiagonal - base.Point,
                    Center = (base.Point + horizontalDiagonal) / 2
                };

                //Vertical line
                Vector2 verticalDiagonal = this.GetVerticalDiagonal(layer, matrix);
                this.VerticalLine = new VectorLine
                {
                    Diagonal = verticalDiagonal,
                    Symmetric = verticalDiagonal + verticalDiagonal - base.Point,
                    Center = (base.Point + verticalDiagonal) / 2,
                };
            }


            public new void Delta(Vector2 point, Layer layer, Matrix3x2 matrix, float scale)
            {
                //Point on diagonal line
                Vector2 footPoint = Transformer.FootPoint(point, this.Line.Diagonal, base.Point);
                VectorDistance distance = base.GetVectorDistance(footPoint, base.Point, this.Line);

                Vector2 point2 = this.IsRatio ? footPoint : point;

                //Point on horizontal line
                Vector2 horizontalFootPoint = Transformer.IntersectionPoint(base.Point, this.HorizontalLine.Diagonal, point2, point2 + this.VerticalLine.Diagonal - base.Point);
                VectorDistance horizontalDistance = base.GetVectorDistance(horizontalFootPoint, base.Point, this.HorizontalLine);

                //Point on vertical line
                Vector2 verticalFootPoint = Transformer.IntersectionPoint(base.Point, this.VerticalLine.Diagonal, point2, point2 + this.HorizontalLine.Diagonal - base.Point);
                VectorDistance verticalDistance = base.GetVectorDistance(verticalFootPoint, base.Point, this.VerticalLine);


                //Scale with Center
                if (this.IsCenter)
                {

                    //Ratio
                    if (this.IsRatio)
                    {
                        //Scale
                        float scale1 = distance.FC / distance.PC;
                        this.SetScale(layer, scale1, scale1);

                        //Flip              
                        bool isFlip = distance.FD > distance.FP;
                        this.SetFlip(layer, isFlip, isFlip);
                    }
                    //Free
                    else
                    {
                        //Scale
                        float xScale = horizontalDistance.FC / horizontalDistance.PC;
                        float yScale = verticalDistance.FC / verticalDistance.PC;
                        this.SetScale(layer, xScale, yScale);

                        //Flip
                        bool isFlipHorizontal = horizontalDistance.FD > horizontalDistance.FP;
                        bool isFlipVertical = verticalDistance.FD > verticalDistance.FP;
                        this.SetFlip(layer, isFlipHorizontal, isFlipVertical);
                    }

                }


                //Scale with Side
                else
                {

                    //Ratio
                    if (this.IsRatio)
                    {
                        //Scale
                        float scale1 = distance.FD / distance.PD;
                        this.SetScale(layer, scale1, scale1);

                        //Flip
                        bool isFlip = distance.FS > distance.FP;
                        this.SetFlip(layer, isFlip, isFlip);
                    }
                    //Free
                    else
                    {
                        //Scale
                        float xScale = horizontalDistance.FD / horizontalDistance.PD;
                        float yScale = verticalDistance.FD / verticalDistance.PD;
                        this.SetScale(layer, xScale, yScale);

                        //Flip
                        bool isFlipHorizontal = horizontalDistance.FS > horizontalDistance.FP;
                        bool isFlipVertical = verticalDistance.FS > verticalDistance.FP;
                        this.SetFlip(layer, isFlipHorizontal, isFlipVertical);
                    }

                    //Postion
                    float xMove = horizontalDistance.FP / 2;
                    float yMove = verticalDistance.FP / 2;
                    VectorSinCos sinCos = new VectorSinCos
                    (
                        base.SinCos.XCos * xMove, base.SinCos.XSin * xMove,
                        base.SinCos.YCos * yMove, base.SinCos.YSin * yMove
                    );
                    sinCos = this.GetReversePostion(horizontalDistance, verticalDistance, sinCos);
                    sinCos = this.GetFlipPostion(sinCos);
                    this.SetPostion(layer, this.StartTransformer, sinCos, scale);
                }
            }


            //Scale
            public void SetScale(Layer layer, float xScale, float yScale)
            {
                layer.Transformer.XScale = this.StartTransformer.XScale * xScale;
                layer.Transformer.YScale = this.StartTransformer.YScale * yScale;
            }
            //Flip
            public void SetFlip(Layer layer, bool isFlipHorizontal, bool isFlipVertical)
            {
                bool flipHorizontal = (this.StartTransformer.XScale < 0) == isFlipHorizontal;
                layer.Transformer.XScale = flipHorizontal ? -layer.Transformer.XScale : layer.Transformer.XScale;

                bool flipVertical = (this.StartTransformer.YScale < 0) == isFlipVertical;
                layer.Transformer.YScale = flipVertical ? -layer.Transformer.YScale : layer.Transformer.YScale;
            }


            //Postion: Reverse
            public VectorSinCos GetReversePostion(VectorDistance horizontalDistance, VectorDistance verticalDistance, VectorSinCos sinCos)
            {
                bool isXReverse = horizontalDistance.FD < horizontalDistance.PD ? true : horizontalDistance.FD < horizontalDistance.FP;
                bool isYReverse = verticalDistance.FD < verticalDistance.PD ? true : verticalDistance.FD < verticalDistance.FP;

                return VectorSinCos.Reverse(sinCos, isXReverse, isYReverse);
            }
            //Postion: Flip
            public VectorSinCos GetFlipPostion(VectorSinCos sinCos)
            {
                bool isFlipHorizontal = this.StartTransformer.XScale < 0;
                bool isFlipVertical = this.StartTransformer.YScale < 0;

                return VectorSinCos.Reverse(sinCos, isFlipHorizontal, isFlipVertical);
            }
        }
        public class ScaleLeftTopController : ScaleCornerController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
            public override Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
            public override Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);

            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + (-sinCos.XCos - sinCos.YSin) / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + (sinCos.XSin - sinCos.YCos) / scale;
            }
        }
        public class ScaleRightTopController : ScaleCornerController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
            public override Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
            public override Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);

            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + (sinCos.XCos - sinCos.YSin) / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + (-sinCos.XSin - sinCos.YCos) / scale;
            }
        }
        public class ScaleRightBottomController : ScaleCornerController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);
            public override Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
            public override Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);

            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + (sinCos.XCos + sinCos.YSin) / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + (-sinCos.XSin + sinCos.YCos) / scale;
            }
        }
        public class ScaleLeftBottomController : ScaleCornerController
        {
            public override Vector2 GetPoint(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftBottom(matrix);
            public override Vector2 GetDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightTop(matrix);
            public override Vector2 GetHorizontalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformRightBottom(matrix);
            public override Vector2 GetVerticalDiagonal(Layer layer, Matrix3x2 matrix) => layer.Transformer.TransformLeftTop(matrix);

            public override void SetPostion(Layer layer, Transformer startTransformer, VectorSinCos sinCos, float scale)
            {
                layer.Transformer.Postion.X = startTransformer.Postion.X + (-sinCos.XCos + sinCos.YSin) / scale;
                layer.Transformer.Postion.Y = startTransformer.Postion.Y + (sinCos.XSin + sinCos.YCos) / scale;
            }
        }


        #endregion


    }
}
