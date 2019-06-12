using System;
using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary> Represents a Transformer (LeftTop, RightTop, RightBottom, LeftBottom). </summary>
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
        /// <summary>
        /// Constructs a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="left"> Transformer's left. </param>
        /// <param name="top"> Transformer's top. </param>
        /// <param name="right"> Transformer's right. </param>
        /// <param name="bottom"> Transformer's bottom. </param>
        public Transformer(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        /// <summary>
        /// Constructs a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="rect"> Transformer's initial rectangle. </param>
        public Transformer(TransformerRect rect)
        {
            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Constructs a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="pointA"> Frist point of transformer.</param>
        /// <param name="pointA"> Second point of transformer.</param>
        public Transformer(Vector2 pointA, Vector2 pointB)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }
        
        /// <summary>
        /// Constructs a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="pointA"> Frist point of rectangle.</param>
        /// <param name="pointA"> Second point of rectangle.</param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        public Transformer(Vector2 pointA, Vector2 pointB, bool isCenter, bool isRatio)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB, isCenter, isRatio);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Constructs a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="width"> Width of transformer.</param>
        /// <param name="height"> Height transformer.</param>
        /// <param name="postion"> Postion of transformer. </param>
        public Transformer(float width, float height, Vector2 postion)
        {
            this.LeftTop = postion;
            this.RightTop = new Vector2(postion.X + width, postion.Y);
            this.RightBottom = new Vector2(postion.X + width, postion.Y + height);
            this.LeftBottom = new Vector2(postion.X, postion.Y + height);
        }



        /// <summary> Gets the center vector. </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 4;

        /// <summary> Gets the center left vector. </summary>
        public Vector2 CenterLeft => (this.LeftTop + this.LeftBottom) / 2;
        /// <summary> Gets the center top vector. </summary>
        public Vector2 CenterTop => (this.LeftTop + this.RightTop) / 2;
        /// <summary> Gets the center right vector. </summary>
        public Vector2 CenterRight => (this.RightTop + this.RightBottom) / 2;
        /// <summary> Gets the center bottom vector. </summary>
        public Vector2 CenterBottom => (this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the minimum value on the X-Axis. </summary>
        public float MinX => Math.Min(Math.Min(this.LeftTop.X, this.RightTop.X), Math.Min(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the maximum  value on the X-Axis. </summary>
        public float MaxX => Math.Max(Math.Max(this.LeftTop.X, this.RightTop.X), Math.Max(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the minimum value on the Y-Axis. </summary>
        public float MinY => Math.Min(Math.Min(this.LeftTop.Y, this.RightTop.Y), Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        /// <summary> Gets the maximum  value on the Y-Axis. </summary>
        public float MaxY => Math.Max(Math.Max(this.LeftTop.Y, this.RightTop.Y), Math.Max(this.RightBottom.Y, this.LeftBottom.Y));

        /// <summary> Gets horizontal vector. </summary>
        public Vector2 Horizontal => (this.RightTop + this.RightBottom - this.LeftTop - this.LeftBottom) / 2;
        /// <summary> Gets vertical vector. </summary>
        public Vector2 Vertical => (this.RightBottom + this.LeftBottom - this.LeftTop - this.RightTop) / 2;


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