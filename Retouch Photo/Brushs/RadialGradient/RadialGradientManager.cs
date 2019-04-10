using System;
using System.Numerics;

namespace Retouch_Photo.Brushs.RadialGradient
{
    public class RadialGradientManager
    {
        public RadialGradientType Type;

        #region Outer


        public Matrix3x2 Transform { get; private set; }
        private Matrix3x2 GetTransform(Vector2 center, Vector2 xPoint) => Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateRotation(RadialGradientManager.VectorToRadians(xPoint - center)) * Matrix3x2.CreateTranslation(center);

        public float RadiusX { get; private set; }
        private float GetRadiusX(Vector2 center, Vector2 xPoint) => Vector2.Distance(center, xPoint);

        public float RadiusY { get; private set; }
        private float GetRadiusY(Vector2 center, Vector2 yPoint) => Vector2.Distance(center, yPoint);


        #endregion

        #region Inner


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

                //Outer
                this.Transform = this.GetTransform(value, this.xPoint);

                this.center = value;
            }
        }

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
                this.Transform = this.GetTransform(this.center, value);
                this.RadiusX = this.GetRadiusX(this.center, value);

                this.xPoint = value;
            }
        }

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
                this.Transform = this.GetTransform(this.center, this.xPoint);
                this.RadiusY = this.GetRadiusY(this.center, value);

                this.yPoint = value;
            }
        }


        #endregion

        public RadialGradientManager(Vector2 center, Vector2 xPoint, Vector2 yPoint) => this.Initialize(center, xPoint, yPoint);
        public void Initialize(Vector2 center, Vector2 xPoint, Vector2 yPoint)
        {
            //Inner
            this.center = center;
            this.xPoint = xPoint;
            this.yPoint = yPoint;

            //Outer
            this.Transform = this.GetTransform(center, xPoint);
            this.RadiusX = this.GetRadiusX(this.center, xPoint);
            this.RadiusY = this.GetRadiusY(this.center, yPoint);
        }

        public RadialGradientManager(Vector2 center, float radiusX, float radiusY) => this.Initialize(center, radiusX, radiusY);
        public void Initialize(Vector2 center, float radiusX, float radiusY)
        {
            //Inner
            this.center = center;
            this.xPoint = new Vector2(center.X + radiusX, center.Y);
            this.yPoint = new Vector2(center.X, center.Y - radiusY);

            //Outer
            this.Transform = this.GetTransform(center, this.xPoint);
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
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
    }
}
