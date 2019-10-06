using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers.Models;
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

        public bool IsRefactoringTransformer { get; set; }
        public virtual Transformer GetActualDestinationWithRefactoringTransformer => this.TransformManager.IsCrop ? this.TransformManager.CropDestination : this.TransformManager.Destination;

        public TransformManager TransformManager { get; set; } = new TransformManager();
        public EffectManager EffectManager { get; set; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; set; } = new AdjustmentManager();


        //@Virtual
        public virtual Color? FillColor { get; set; } = null;
        public virtual Color? StrokeColor { get; set; } = null;
        

        //@Virtual
        public virtual void CacheTransform()
        {
            //RefactoringTransformer
            if (this.parents != null)
            {
                if (this.parents is GroupLayer groupLayer)
                {
                    groupLayer.IsRefactoringTransformer = true;
                }
            }

            foreach (ILayer child in this.Children)
            {
                child.TransformManager.CacheTransform();
            }
            this.TransformManager.CacheTransform();
        }
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            foreach (ILayer child in this.Children)
            {
                child.TransformManager.TransformMultiplies(matrix);
            }
            this.TransformManager.TransformMultiplies(matrix);
        }
        public virtual void TransformAdd(Vector2 vector)
        {
            foreach (ILayer child in this.Children)
            {
                child.TransformManager.TransformAdd(vector);
            }
            this.TransformManager.TransformAdd(vector);
        }


        //@Abstract
        public abstract ILayer Clone(ICanvasResourceCreator resourceCreator);


        //@Static
        /// <summary>
        /// Copy a layer with self.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="source"> The source ILayer. </param>
        /// <param name="destination"> The destination ILayer. </param>
        public static void CopyWith(ICanvasResourceCreator resourceCreator, ILayer destination, ILayer source)
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
                ILayer clone = layer.Clone(resourceCreator);
                destination.Children.Add(clone);
            }
        }

    }
}