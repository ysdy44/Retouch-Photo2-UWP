using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        //@Abstract
        public string Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Opacity { get; set; } = 1.0f;
        public BlendType BlendType { get; set; } = BlendType.None;

        private Visibility visibility;
        public Visibility Visibility
        {
            get => this.visibility;
            set
            {
                this.Control.SetVisibility(value);
                this.visibility = value;
            }
        }
        private TagType tagType;
        public TagType TagType
        {
            get => this.tagType;
            set
            {
                this.Control.SetTagType(value);
                this.tagType = value;
            }
        }

        public bool IsRefactoringTransformer { get; set; }
        public virtual Transformer GetActualDestinationWithRefactoringTransformer => this.TransformManager.IsCrop ? this.TransformManager.CropDestination : this.TransformManager.Destination;

        public TransformManager TransformManager { get; set; } = new TransformManager();
        public EffectManager EffectManager { get; set; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; set; } = new AdjustmentManager();
        public StyleManager StyleManager { get; set; } = new StyleManager();
        
        private ILayer parents;
        public ILayer Parents
        {
            get => this.parents;
            set
            {
                int depth = (value == null) ? 0 : value.Control.Depth + 1; //+1

                this.Control.Depth = depth;
                foreach (ILayer child in this.Children)
                {
                    child.Control.Depth = depth + 1; //+1
                }

                this.parents = value;
            }
        }
        public IList<ILayer> Children { get; set; } = new List<ILayer>();

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
                child.CacheTransform();
            }
            this.TransformManager.CacheTransform();
            this.StyleManager.CacheTransform();
        }
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            foreach (ILayer child in this.Children)
            {
                child.TransformMultiplies(matrix);
            }
            this.TransformManager.TransformMultiplies(matrix);
            this.StyleManager.TransformMultiplies(matrix);
        }
        public virtual void TransformAdd(Vector2 vector)
        {
            foreach (ILayer child in this.Children)
            {
                child.TransformAdd(vector);
            }
            this.TransformManager.TransformAdd(vector);
            this.StyleManager.TransformAdd(vector);
        }

    }
}