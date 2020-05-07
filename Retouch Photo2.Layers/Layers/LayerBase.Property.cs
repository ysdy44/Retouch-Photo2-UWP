using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Layers.Models;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
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
        public abstract LayerType Type { get; }
        public string Name { get; set; } = string.Empty;
        public float Opacity { get; set; } = 1.0f;
        public BlendEffectMode? BlendMode { get; set; } = null;

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

        public StyleManager StyleManager { get; set; } = new StyleManager();
        public TransformManager TransformManager { get; set; } = new TransformManager();
        public EffectManager EffectManager { get; set; } = new EffectManager();
        public AdjustmentManager AdjustmentManager { get; set; } = new AdjustmentManager();
        
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


        public abstract ILayer Clone(ICanvasResourceCreator resourceCreator);

        public virtual void SaveWith(XElement element) { }
        public virtual void Load(XElement element) { }


        public virtual void CacheTransform()
        {
            this.StyleManager.CacheTransform();
            this.TransformManager.CacheTransform();

            //RefactoringTransformer
            if (this.parents != null)
            {
                if (this.parents.Type  == LayerType.Group)
                {
                    GroupLayer groupLayer = (GroupLayer)this.parents;
                    groupLayer.IsRefactoringTransformer = true;
                }
            }
        }
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            this.StyleManager.TransformMultiplies(matrix);
            this.TransformManager.TransformMultiplies(matrix);
        }
        public virtual void TransformAdd(Vector2 vector)
        {
            this.StyleManager.TransformAdd(vector);
            this.TransformManager.TransformAdd(vector);
        }
        

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
            destination.BlendMode = source.BlendMode;
            destination.Visibility = source.Visibility;

            destination.StyleManager = source.StyleManager.Clone();
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