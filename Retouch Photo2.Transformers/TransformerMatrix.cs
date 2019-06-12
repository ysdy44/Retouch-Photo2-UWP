using System.Numerics;

namespace Retouch_Photo2.Transformers
{
    /// <summary> Contains two <see cref = "Transformer" />s (Source and Destination). </summary>
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

            this.OldDestination = transformer;

            this.DdisabledRadian = false;
        }
        /// <summary>
        /// Constructs a <see cref = "TransformerMatrix" />.
        /// </summary>
        /// <param name="pointA"> Frist point of transformer matrix.</param>
        /// <param name="pointA"> Second point of transformer matrix.</param>
        public TransformerMatrix(Vector2 pointA, Vector2 pointB)
        {
            Transformer transformer = new Transformer(pointA, pointB);

            //Source
            this.Source = transformer;

            //Destination
            this.Destination = transformer;

            this.OldDestination = transformer;

            this.DdisabledRadian = false;
        }
        /// <summary>
        /// Constructs a <see cref = "TransformerMatrix" />. 
        /// </summary> 
        /// <param name="width"> Width of transformer matrix.</param>
        /// <param name="height"> Height transformer matrix.</param>
        /// <param name="postion"> Postion of transformer matrix. </param>
        public TransformerMatrix(float width, float height, Vector2 postion)
        {
            Transformer transformer = new Transformer(width, height,postion);

            //Source
            this.Source = transformer;

            //Destination
            this.Destination = transformer;

            this.OldDestination = transformer;

            this.DdisabledRadian = false;
        }


        /// <summary>
        /// Gets <see cref = "TransformerMatrix" />'s matrix.
        /// </summary>
        /// <returns> matrix </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);
  }
}