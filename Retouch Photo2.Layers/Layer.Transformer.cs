using Retouch_Photo2.Transformers;
using System.ComponentModel;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Layer Classes.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {
        //@Virtual
      //  public virtual void CacheTransformerMatrix() => this.TransformerMatrix.OldDestination = this.TransformerMatrix.Destination;
       // public virtual void MultipliesTransformerMatrix(Matrix3x2 matrix) => this.TransformerMatrix.Destination = Transformer.Multiplies(this.TransformerMatrix.OldDestination, matrix);


        /// <summary> <see cref = "Layer" />'s TransformerMatrix. </summary>
        public TransformerMatrix TransformerMatrix;

    }
}