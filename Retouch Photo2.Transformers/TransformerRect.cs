using System;
using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary> Represents a Rect (Left, Top, Right, Bottom). </summary>
    public struct TransformerRect
    {

        /// <summary> Gets rectangle's left. </summary>
        public float Left { get; private set; }
        /// <summary> Gets rectangle's top. </summary>
        public float Top { get; private set; }
        /// <summary> Gets rectangle's right. </summary>
        public float Right { get; private set; }
        /// <summary> Gets rectangle's bottom. </summary>
        public float Bottom { get; private set; }

        /// <summary> Gets rectangle's left-top point. </summary>
        public Vector2 LeftTop { get; private set; }
        /// <summary> Gets rectangle's right-top point. </summary>
        public Vector2 RightTop { get; private set; }
        /// <summary> Gets rectangle's right-bottom point. </summary>
        public Vector2 RightBottom { get; private set; }
        /// <summary> Gets rectangle's left-bottom point. </summary>
        public Vector2 LeftBottom { get; private set; }

        //@Constructs
        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="pointA"> Frist point of rectangle.</param>
        /// <param name="pointA"> Second point of rectangle.</param>
        public TransformerRect(Vector2 pointA, Vector2 pointB)
        {
            float left = Math.Min(pointA.X, pointB.X);
            float top = Math.Min(pointA.Y, pointB.Y);
            float right = Math.Max(pointA.X, pointB.X);
            float bottom = Math.Max(pointA.Y, pointB.Y);

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }

        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="pointA"> Frist point of rectangle.</param>
        /// <param name="pointA"> Second point of rectangle.</param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        public TransformerRect(Vector2 pointA, Vector2 pointB, bool isCenter, bool isRatio)
        {
            if (isRatio)
            {
                float square = Vector2.Distance(pointA, pointB) / 1.4142135623730950488016887242097f;

                pointB = pointA + new Vector2((pointB.X > pointA.X) ? square : -square, (pointB.Y > pointA.Y) ? square : -square);
            }

            if (isCenter)
            {
                pointA = pointA + pointA - pointB;
            }

            float left = Math.Min(pointA.X, pointB.X);
            float top = Math.Min(pointA.Y, pointB.Y);
            float right = Math.Max(pointA.X, pointB.X);
            float bottom = Math.Max(pointA.Y, pointB.Y);

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }


        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="width"> Width of rectangle.</param>
        /// <param name="height"> Height rectangle.</param>
        /// <param name="postion"> Postion of rectangle. </param>
        public TransformerRect(float width, float height, Vector2 postion)
        {
            float left = postion.X;
            float top = postion.Y;
            float right = width + postion.X;
            float bottom = height+ postion.Y;
            
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }

    }
}