using System.ComponentModel;
using Windows.UI;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Layer Classes.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {
        //@Virtual
        /// <summary>
        /// Sets layer's fill-color.
        /// </summary>
        /// <param name="fillColor"> The destination fill-color. </param>
        public virtual void SetFillColor(Color fillColor) { }

        /// <summary>
        /// Gets layer's fill-color.
        /// </summary>
        /// <returns> Return **Null** if layer does not have fill-color. </returns>
        public virtual Color? GetFillColor() => null;

        /// <summary>
        /// Sets layer's stroke-color.
        /// </summary>
        /// <param name="fillColor"> The destination stroke-color. </param>
        public virtual void SetStrokeColor(Color strokeColor) { }

        /// <summary>
        /// Gets layer's stroke-color.
        /// </summary>
        /// <returns> Return **Null** if layer does not have stroke-color. </returns>
        public virtual Color? GetStrokeColor() => null;
    }
}