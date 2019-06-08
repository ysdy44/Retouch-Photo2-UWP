using System;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define Transformer. </summary>
    public partial struct TransformerMatrix
    {
        /// <summary> The source Transformer. </summary>
        public Transformer Source;

        /// <summary> The destination Transformer. </summary>
        public Transformer Destination;

        /// <summary> <see cref = "TransformerMatrix.Destination" />'s old cache. </summary>
        public Transformer OldDestination;

        /// <summary> Is disable rotate radian? </summary>
        public bool DdisabledRadian;


        //@Constructs
        /// <summary> Constructs a <see cref = "TransformerMatrix" />. </summary>
        public TransformerMatrix(Transformer transformer)
        {
            //Source
            this.Source = transformer;
            //Destination
            this.Destination = transformer;

            this.OldDestination = new Transformer();

            this.DdisabledRadian = false;
        }
        /// <summary> Constructs a <see cref = "TransformerMatrix" />. </summary>
        public TransformerMatrix(Vector2 leftTop, Vector2 rightBottom)
        {
            //Source
            this.Source.LeftTop = leftTop;
            this.Source.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.Source.RightBottom = rightBottom;
            this.Source.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);
            //Destination
            this.Destination.LeftTop = leftTop;
            this.Destination.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.Destination.RightBottom = rightBottom;
            this.Destination.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);

            this.OldDestination = new Transformer();

            this.DdisabledRadian = false;
        }
        /// <summary> Constructs a <see cref = "TransformerMatrix" />. </summary>
        public TransformerMatrix(float width, float height, Vector2 postion, float scale = 1, bool disabledRadian = false)
        {
            //Source
            this.Source.LeftTop = Vector2.Zero;
            this.Source.RightTop = new Vector2(width, 0);
            this.Source.RightBottom = new Vector2(width, height);
            this.Source.LeftBottom = new Vector2(0, height);
            //Destination
            this.Destination.LeftTop = postion;
            this.Destination.RightTop = postion + new Vector2(width * scale, 0);
            this.Destination.RightBottom = postion + new Vector2(width * scale, height * scale);
            this.Destination.LeftBottom = postion + new Vector2(0, height * scale);

            this.OldDestination = new Transformer();

            this.DdisabledRadian = disabledRadian;
        }


        /// <summary>
        /// Gets <see cref = "TransformerMatrix" />'s matrix.
        /// </summary>
        /// <returns> matrix </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);
  }
}