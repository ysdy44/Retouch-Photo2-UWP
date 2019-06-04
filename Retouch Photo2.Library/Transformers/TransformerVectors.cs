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