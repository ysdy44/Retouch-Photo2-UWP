using System;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define Transformer. </summary>
    public partial struct Transformer
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
        /// <summary> Constructs a <see cref = "Transformer" />. </summary>
        public Transformer(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        /// <summary> Constructs a <see cref = "Transformer" />. </summary>
        public Transformer(Vector2 leftTop, Vector2 rightBottom)
        {
            this.LeftTop = leftTop;
            this.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.RightBottom = rightBottom;
            this.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);
        }



        /// <summary> Gets the center vector. / </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 4;

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
        /// Adds <see cref = "Transformer" /> and vector.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="vector"> The added vector. </param>
        /// <returns> Transformer </returns>
        public static Transformer Add(Transformer transformer, Vector2 vector) => new Transformer
        {
            LeftTop = transformer.LeftTop + vector,
            RightTop = transformer.RightTop + vector,
            RightBottom = transformer.RightBottom + vector,
            LeftBottom = transformer.LeftBottom + vector,
        };

        /// <summary>
        /// Multiplies <see cref = "Transformer" /> and vector.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The transformation matrix. </param>
        /// <returns> Transformer </returns>
        public static Transformer Multiplies(Transformer transformer, Matrix3x2 matrix) => new Transformer
        {
            LeftTop = Vector2.Transform(transformer.LeftTop, matrix),
            RightTop = Vector2.Transform(transformer.RightTop, matrix),
            RightBottom = Vector2.Transform(transformer.RightBottom, matrix),
            LeftBottom = Vector2.Transform(transformer.LeftBottom, matrix)
        };
    }
}