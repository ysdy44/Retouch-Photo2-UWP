using System;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define TransformerVectors. </summary>
    public struct TransformerVectors
    {
        /// <summary> Vector in LeftTop. </summary>
        public Vector2 LeftTop;
        /// <summary> Vector in RightTop. </summary>
        public Vector2 RightTop;
        /// <summary> Vector in RightBottom. </summary>
        public Vector2 RightBottom;
        /// <summary> Vector in LeftBottom. </summary>
        public Vector2 LeftBottom;



        //@Constructs
        /// <summary> Constructs a <see cref = "TransformerVectors" />. </summary>
        public TransformerVectors(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        /// <summary> Constructs a <see cref = "TransformerVectors" />. </summary>
        public TransformerVectors(Vector2 leftTop, Vector2 rightBottom)
        {
            this.LeftTop = leftTop;
            this.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.RightBottom = rightBottom;
            this.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);
        }
        /// <summary> Constructs a <see cref = "TransformerVectors" />. </summary>
        public TransformerVectors(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }



        /// <summary> Gets the center vector. / </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the center left vector. / </summary>
        public Vector2 CenterLeft => (this.LeftTop + this.LeftBottom) / 2;
        /// <summary> Gets the center top vector. / </summary>
        public Vector2 CenterTop => (this.LeftTop + this.RightTop) / 2;
        /// <summary> Gets the center right vector. / </summary>
        public Vector2 CenterRight => (this.RightTop + this.RightBottom) / 2;
        /// <summary> Gets the center bottom vector. / </summary>
        public Vector2 CenterBottom => (this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the minimum value on the X-Axis. </summary>
        public float MinX => Math.Min(Math.Min(this.LeftTop.X, this.RightTop.X), Math.Min(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the maximum  value on the X-Axis. </summary>
        public float MaxX => Math.Max(Math.Max(this.LeftTop.X, this.RightTop.X), Math.Max(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the minimum value on the Y-Axis. </summary>
        public float MinY => Math.Min(Math.Min(this.LeftTop.Y, this.RightTop.Y), Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        /// <summary> Gets the maximum  value on the Y-Axis. </summary>
        public float MaxY => Math.Max(Math.Max(this.LeftTop.Y, this.RightTop.Y), Math.Max(this.RightBottom.Y, this.LeftBottom.Y));



        //@Static
        /// <summary>
        /// Adds vectors and vector.
        /// </summary>
        /// <param name="vectors"> The source vectors. </param>
        /// <param name="vector"> The added vector. </param>
        /// <returns> vectors </returns>
        public static TransformerVectors Add(TransformerVectors vectors, Vector2 vector) => new TransformerVectors
        {
            LeftTop = vectors.LeftTop + vector,
            RightTop = vectors.RightTop + vector,
            RightBottom = vectors.RightBottom + vector,
            LeftBottom = vectors.LeftBottom + vector,
        };

        /// <summary>
        /// Multiplies vectors and vector.
        /// </summary>
        /// <param name="vectors"> The source vectors. </param>
        /// <param name="matrix"> The transformation matrix. </param>
        /// <returns> vectors </returns>
        public static TransformerVectors Multiplies(TransformerVectors vectors, Matrix3x2 matrix) => new TransformerVectors
        {
            LeftTop = Vector2.Transform(vectors.LeftTop, matrix),
            RightTop = Vector2.Transform(vectors.RightTop, matrix),
            RightBottom = Vector2.Transform(vectors.RightBottom, matrix),
            LeftBottom = Vector2.Transform(vectors.LeftBottom, matrix)
        };
    }
}