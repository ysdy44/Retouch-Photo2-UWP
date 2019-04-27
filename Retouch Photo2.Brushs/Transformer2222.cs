using Microsoft.Graphics.Canvas;
using System;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary> Define Transformer. </summary>
    public struct Transformer2222
    {
         

        #region Contains        


        /// <summary> Radius of node'. Defult 12. </summary>
        private const float NodeRadius = 12.0f;
        /// <summary> Whether the distance exceeds [NodeRadius]. Defult: 144. </summary>
        public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;


        /// <summary> Minimum distance between two nodes. Defult 20. </summary>
        public const float NodeDistance = 20.0f;
        /// <summary> Double [NodeDistance]. Defult 40. </summary>
        public const float NodeDistanceDouble = 40.0f;
        /// <summary> Whether the distance'LengthSquared exceeds [NodeDistance]. Defult: 400. </summary>
        public static bool OutNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;


        /// <summary> Get outside node. </summary>
        private static Vector2 OutsideNode(Vector2 nearNode, Vector2 farNode) => nearNode - Vector2.Normalize(farNode - nearNode) * Transformer2222.NodeDistanceDouble;


        /// <summary> Point inside the Quadrangle. </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Is point in quadrangle? </returns>
        public static bool InQuadrangle(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            float a = (leftTop.X - leftBottom.X) * (point.Y - leftBottom.Y) - (leftTop.Y - leftBottom.Y) * (point.X - leftBottom.X);
            float b = (rightTop.X - leftTop.X) * (point.Y - leftTop.Y) - (rightTop.Y - leftTop.Y) * (point.X - leftTop.X);
            float c = (rightBottom.X - rightTop.X) * (point.Y - rightTop.Y) - (rightBottom.Y - rightTop.Y) * (point.X - rightTop.X);
            float d = (leftBottom.X - rightBottom.X) * (point.Y - rightBottom.Y) - (leftBottom.Y - rightBottom.Y) * (point.X - rightBottom.X);
            return (a > 0 && b > 0 && c > 0 && d > 0) || (a < 0 && b < 0 && c < 0 && d < 0);
        }

         

        #endregion


        #region Draw

         
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


        #endregion


        #region Vector


        /// <summary> 15 degress in angle system. </summary>
        public const float RadiansStep = 0.2617993833333333f;
        /// <summary> 7.5 degress in angle system. </summary>
        public const float RadiansStepHalf = 0.1308996916666667f;
        /// <summary> To find a multiple of the nearest 15. </summary>
        public static float RadiansStepFrequency(float radian) => ((int)((radian + Transformer2222.RadiansStepHalf) / Transformer2222.RadiansStep)) * Transformer2222.RadiansStep;//Get step radians


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
        public static Vector2 RadiansToVector(float radians, Vector2 center, float length = 40.0f) => new Vector2((float)Math.Cos(radians) * length + center.X, (float)Math.Sin(radians) * length + center.Y);

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