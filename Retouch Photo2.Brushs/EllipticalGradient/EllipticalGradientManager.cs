using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Brushs.EllipticalGradient
{
    public class EllipticalGradientManager: IGradientManager
    {
        public EllipticalGradientType Type;

        #region Gradient Outer


        public Matrix3x2 GetTransform(Vector2 center, Vector2 xPoint) => Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateRotation(this.VectorToRadians(xPoint - center)) * Matrix3x2.CreateTranslation(center);

        private float RadiusX;
        public float GetRadiusX(Vector2 center, Vector2 xPoint) => Vector2.Distance(center, xPoint);

        private float RadiusY;
        public float GetRadiusY(Vector2 center, Vector2 yPoint) => Vector2.Distance(center, yPoint);


        #endregion

        #region Gradient Inner


        private Vector2 center;
        public Vector2 Center
        {
            get => this.center;
            set
            {
                //Inner
                Vector2 vector = value - this.center;
                this.xPoint += vector;
                this.yPoint += vector;

                this.center = value;
            }
        }
        public Vector2 OldCenter;


        private Vector2 xPoint;
        public Vector2 XPoint
        {
            get => this.xPoint;
            set
            {
                //Inner
                Vector2 normalize = Vector2.Normalize(value - this.center);
                this.yPoint = this.center + this.RadiusY * new Vector2(normalize.Y, -normalize.X);

                //Outer
                this.RadiusX = this.GetRadiusX(this.center, value);

                this.xPoint = value;
            }
        }
        public Vector2 OldXPoint;


        private Vector2 yPoint;
        public Vector2 YPoint
        {
            get => this.yPoint;
            set
            {
                //Inner
                Vector2 normalize = Vector2.Normalize(value - this.center);
                this.xPoint = this.center + this.RadiusX * new Vector2(-normalize.Y, normalize.X);

                //Outer
                this.RadiusY = this.GetRadiusY(this.center, value);

                this.yPoint = value;
            }
        }
        public Vector2 OldYPoint;

        #endregion

        public EllipticalGradientManager() { }

        public EllipticalGradientManager(Vector2 center, Vector2 xPoint, Vector2 yPoint) => this.Initialize(center, xPoint, yPoint);
        public void Initialize(Vector2 center, Vector2 xPoint, Vector2 yPoint)
        {
            //Inner
            this.center = center;
            this.xPoint = xPoint;
            this.yPoint = yPoint;

            //Outer
            this.RadiusX = this.GetRadiusX(this.center, xPoint);
            this.RadiusY = this.GetRadiusY(this.center, yPoint);
        }

        public EllipticalGradientManager(Vector2 center, float radiusX, float radiusY) => this.Initialize(center, radiusX, radiusY);
        public void Initialize(Vector2 center, float radiusX, float radiusY)
        {
            //Inner
            this.center = center;
            this.xPoint = new Vector2(center.X + radiusX, center.Y);
            this.yPoint = new Vector2(center.X, center.Y - radiusY);

            //Outer
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
        }

        public ICanvasBrush GetBrush(ICanvasResourceCreator creator, Matrix3x2 matrix,CanvasGradientStop[] array)
        {
            Vector2 center = Vector2.Transform(this.Center, matrix);
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            return new CanvasRadialGradientBrush(creator, array)
            {
                Transform = this.GetTransform(center, xPoint),
                RadiusX = this.GetRadiusX(center, xPoint),
                RadiusY = this.GetRadiusY(center, yPoint),
                Center = center
            };
        }


        #region Tool


        public void Start(Vector2 point, Matrix3x2 matrix)
        {
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            if (Vector2.DistanceSquared(point, xPoint) < 400)
            {
                this.Type = EllipticalGradientType.XPoint;
                return;
            }

            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            if (Vector2.DistanceSquared(point, yPoint) < 400)
            {
                this.Type = EllipticalGradientType.YPoint;
                return;
            }

            Vector2 center = Vector2.Transform(this.Center, matrix);
            if (Vector2.DistanceSquared(point, center) < 400)
            {
                this.Type = EllipticalGradientType.Center;
                return;
            }
        }
        public void Delta(Vector2 point, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            switch (this.Type)
            {
                case EllipticalGradientType.None:
                    return;

                case EllipticalGradientType.XPoint:
                    this.XPoint = canvasPoint;
                    break;

                case EllipticalGradientType.YPoint:
                    this.YPoint = canvasPoint;
                    break;

                case EllipticalGradientType.Center:
                    this.Center = canvasPoint;
                    break;

                default:
                    return;
            }
        }
        public void Complete() => this.Type = EllipticalGradientType.None;

        public void Draw(CanvasDrawingSession ds, Matrix3x2 matrix)
        {
            Vector2 xPoint = Vector2.Transform(this.XPoint, matrix);
            Vector2 yPoint = Vector2.Transform(this.YPoint, matrix);
            Vector2 center = Vector2.Transform(this.Center, matrix);

            ds.DrawLine(xPoint, center, Colors.DodgerBlue);
            ds.DrawLine(yPoint, center, Colors.DodgerBlue);
            this.DrawNode(ds, xPoint);
            this.DrawNode(ds, yPoint);
            this.DrawNode(ds, center);
        }


        /// <summary> Draw a ⊙. </summary>
        public void DrawNode(CanvasDrawingSession ds, Vector2 vector)
        {
            ds.FillCircle(vector, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(vector, 8, Windows.UI.Colors.DodgerBlue);
            ds.FillCircle(vector, 6, Windows.UI.Colors.White);
        }

        #endregion



        /// <summary> Get radians of the vector in the coordinate system. </summary>
        public float VectorToRadians(Vector2 vector)
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


    }
}
