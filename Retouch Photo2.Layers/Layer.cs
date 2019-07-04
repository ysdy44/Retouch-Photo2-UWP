using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Layer Classes.
    /// </summary>
    public abstract partial class Layer : INotifyPropertyChanged
    {
        //@Abstract
         /// <summary>
        /// Gets layer's icon.
        /// </summary>
        /// <returns> icon </returns>
        public abstract UIElement GetIcon();      
        /// <summary>
        /// Get layer own copy.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <returns></returns>
        public abstract Layer Clone(ICanvasResourceCreator resourceCreator);

        //@Virtual
        /// <summary>
        ///  Cache a layer'a transformer.
        /// </summary>
        public virtual void CacheTransform() => this.TransformerMatrix.OldDestination = this.TransformerMatrix.Destination;
        /// <summary>
        ///  Transforms a layer by the given matrix.
        /// </summary>
        /// <param name="matrix"> The sestination matrix. </param>
        public virtual void TransformMultiplies(Matrix3x2 matrix) => this.TransformerMatrix.Destination = Transformer.Multiplies(this.TransformerMatrix.OldDestination, matrix);
        /// <summary>
        ///  Transforms a layer by the given vector.
        /// </summary>
        /// <param name="vector"> The sestination vector. </param>
        public virtual void TransformAdd(Vector2 vector) => this.TransformerMatrix.Destination = Transformer.Add(this.TransformerMatrix.OldDestination, vector);

        
        /// <summary> <see cref = "Layer" />'s name. </summary>
        public string Name = "Layer";
        /// <summary> <see cref = "Layer" />'s icon. </summary>
        public UIElement Icon=>this.GetIcon();
        /// <summary> <see cref = "Layer" />'s opacity. </summary>
        public float Opacity=1.0f;
        /// <summary> <see cref = "Layer" />'s blend type. </summary>
        public BlendType BlendType;

        /// <summary> <see cref = "Layer" />'s TransformerMatrix. </summary>
        public TransformerMatrix TransformerMatrix;

        /// <summary> <see cref = "Layer" />'s children layers. </summary>
        public ObservableCollection<Layer> Children = new ObservableCollection<Layer>();
        
        /// <summary> <see cref = "Layer" />'s EffectManager. </summary>
        public EffectManager EffectManager = new EffectManager();
        /// <summary> <see cref = "Layer" />'s AdjustmentManager. </summary>
        public AdjustmentManager AdjustmentManager = new AdjustmentManager();
    }
}