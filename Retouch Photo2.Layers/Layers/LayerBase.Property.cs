using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase : ILayer
    {

        //@Abstract
        public abstract string Type { get; }
        public string Name { get; set; } = string.Empty;
        public float Opacity { get; set; } = 1.0f;
        public BlendType BlendType { get; set; } = BlendType.None;
        public Visibility Visibility { get; set; }

        public TransformManager TransformManager { get; set; } = new TransformManager();
        public EffectManager EffectManager { get; set; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; set; } = new AdjustmentManager();


        //@Virtual
        public virtual Color? FillColor { get; set; } = null;
        public virtual Color? StrokeColor { get; set; } = null;
        

        //@Virtual
        public virtual void CacheTransform() => this.TransformManager.CacheTransform();
        public virtual void TransformMultiplies(Matrix3x2 matrix) => this.TransformManager.TransformMultiplies(matrix);
        public virtual void TransformAdd(Vector2 vector) => this.TransformManager.TransformAdd(vector);


        //@Abstract
        public abstract ILayer Clone(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator);


        //@Static
        /// <summary>
        /// Copy a layer with self.
        /// </summary>
        /// <param name="layerCollection"> The layer-collection. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="source"> The source ILayer. </param>
        /// <param name="destination"> The destination ILayer. </param>
        public static void CopyWith(LayerCollection layerCollection, ICanvasResourceCreator resourceCreator, ILayer destination, ILayer source)
        {
            destination.Name = source.Name;
            destination.Opacity = source.Opacity;
            destination.BlendType = source.BlendType;
            destination.Visibility = source.Visibility;

            destination.TransformManager = source.TransformManager.Clone();
            destination.EffectManager = source.EffectManager.Clone();

            foreach (IAdjustment adjustment in source.AdjustmentManager.Adjustments)
            {
                IAdjustment clone = adjustment.Clone();
                destination.AdjustmentManager.Adjustments.Add(clone);
            }
            foreach (ILayer layer in source.Children)
            {
                ILayer clone = layer.Clone(layerCollection, resourceCreator);
                destination.Children.Add(clone);
            }
        }

    }
}