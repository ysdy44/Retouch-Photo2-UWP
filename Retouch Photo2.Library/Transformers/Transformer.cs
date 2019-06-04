using System;
using System.Numerics;

namespace Retouch_Photo2.Library
{
    /// <summary> Define Transformer. </summary>
    public partial struct Transformer
    {
        /// <summary> The source TransformerVectors. </summary>
        public TransformerVectors SourceVectors;

        /// <summary> The destination TransformerVectors. </summary>
        public TransformerVectors DestinationVectors;

        /// <summary> Is disable rotate radian? </summary>
        public bool DdisabledRadian;


        //@Constructs
        /// <summary> Constructs a <see cref = "Transformer" />. </summary>
        public Transformer(TransformerVectors transformerVectors)
        {
            //Source
            this.SourceVectors = transformerVectors;
            //Destination
            this.DestinationVectors = transformerVectors;

            this.DdisabledRadian = false;
        }
        /// <summary> Constructs a <see cref = "Transformer" />. </summary>
        public Transformer(Vector2 leftTop, Vector2 rightBottom)
        {
            //Source
            this.SourceVectors.LeftTop = leftTop;
            this.SourceVectors.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.SourceVectors.RightBottom = rightBottom;
            this.SourceVectors.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);
            //Destination
            this.DestinationVectors.LeftTop = leftTop;
            this.DestinationVectors.RightTop = new Vector2(rightBottom.X, leftTop.Y);
            this.DestinationVectors.RightBottom = rightBottom;
            this.DestinationVectors.LeftBottom = new Vector2(leftTop.X, rightBottom.Y);

            this.DdisabledRadian = false;
        }
        /// <summary> Constructs a <see cref = "Transformer" />. </summary>
        public Transformer(float width, float height, Vector2 postion, float scale = 1, bool disabledRadian = false)
        {
            //Source
            this.SourceVectors.LeftTop = Vector2.Zero;
            this.SourceVectors.RightTop = new Vector2(width, 0);
            this.SourceVectors.RightBottom = new Vector2(width, height);
            this.SourceVectors.LeftBottom = new Vector2(0, height);
            //Destination
            this.DestinationVectors.LeftTop = postion;
            this.DestinationVectors.RightTop = postion + new Vector2(width * scale, 0);
            this.DestinationVectors.RightBottom = postion + new Vector2(width * scale, height * scale);
            this.DestinationVectors.LeftBottom = postion + new Vector2(0, height * scale);

            this.DdisabledRadian = disabledRadian;
        }



        public Matrix3x2 Matrix => Transformer.FindHomography(this.SourceVectors, this.DestinationVectors);
  }
}