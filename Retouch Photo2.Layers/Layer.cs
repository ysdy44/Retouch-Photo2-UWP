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
    public abstract partial class Layer : INotifyPropertyChanged, ICacheTransform
    {
        //@Abstract
        /// <summary> Gets layer's type name. </summary>
        public abstract string Type { get; }
         /// <summary>
        /// Gets layer's icon.
        /// </summary>
        /// <returns> icon </returns>
        public abstract UIElement Icon { get; }
        /// <summary>
        /// Get layer own copy.
        /// </summary>
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <returns> The cloned layer. </returns>
        public abstract Layer Clone(ICanvasResourceCreator resourceCreator);


        //@Interface
        /// <summary>
        ///  Cache a layer's transformer.
        /// </summary>
        public virtual void CacheTransform() => this.OldDestination = this.Destination;
        /// <summary>
        ///  Transforms a layer by the given matrix.
        /// </summary>
        /// <param name="matrix"> The sestination matrix. </param>
        public virtual void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.OldDestination * matrix;
        /// <summary>
        ///  Transforms a layer by the given vector.
        /// </summary>
        /// <param name="vector"> The sestination vector. </param>
        public virtual void TransformAdd(Vector2 vector) => this.Destination = this.OldDestination + vector;



        /// <summary> <see cref = "Layer" />'s name. </summary>
        public string Name;
        /// <summary> <see cref = "Layer" />'s opacity. </summary>
        public float Opacity=1.0f;
        /// <summary> <see cref = "Layer" />'s blend type. </summary>
        public BlendType BlendType;


        
        /// <summary>
        /// Gets transformer-matrix>'s resulting matrix.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);
        /// <summary> The source Transformer. </summary>
        public Transformer Source { get; set; }
        /// <summary> The destination Transformer. </summary>
        public Transformer Destination { get; set; }
        /// <summary> <see cref = "TransformerMatrix.Destination" />'s old cache. </summary>
        public Transformer OldDestination { get; set; }
        /// <summary> Is disable rotate radian? Defult **false**. </summary>
        public bool DisabledRadian { get; set; }



        /// <summary> <see cref = "Layer" />'s children layers. </summary>
        public ObservableCollection<Layer> Children = new ObservableCollection<Layer>();
                /// <summary> <see cref = "Layer" />'s EffectManager. </summary>
        public EffectManager EffectManager = new EffectManager();
        /// <summary> <see cref = "Layer" />'s AdjustmentManager. </summary>
        public AdjustmentManager AdjustmentManager = new AdjustmentManager();
    }
}