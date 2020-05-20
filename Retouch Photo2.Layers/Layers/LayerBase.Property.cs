using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
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
        public BlendEffectMode? BlendMode { get; set; } = null;

        public float Opacity { get; set; } = 1.0f;
        public float StartingOpacity { get; private set; } = 1.0f;
        public void CacheOpacity() => this.StartingOpacity = this.Opacity;

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
        public virtual Transformer GetActualDestinationWithRefactoringTransformer => this.Transform.IsCrop ? this.Transform.CropDestination : this.Transform.Destination;

        public Retouch_Photo2.Brushs.Style Style { get; set; } = new Retouch_Photo2.Brushs.Style();
        public Transform Transform { get; set; } = new Transform();
        public Effect Effect { get; set; } = new Effect();
        public Filter Filter { get; set; } = new Filter();

        public ILayer Parents { get; set; } = null;
        public IList<ILayer> Children { get; set; } = new List<ILayer>();


        public abstract ILayer Clone(ICanvasResourceCreator resourceCreator);

        public virtual void SaveWith(XElement element) { }
        public virtual void Load(XElement element) { }


        public virtual void Fill(IBrush brush) { }
        public virtual void Stroke(IBrush brush) { }
        public virtual void StrokeWidth(float width) { }
        public virtual void StrokeStyle(CanvasStrokeStyle strokeStyle) { }


        public virtual void CacheTransform()
        {
            this.Style.CacheTransform();
            this.Transform.CacheTransform();

            //RefactoringTransformer
            if (this.Parents != null)
            {
                if (this.Parents.Type == LayerType.Group)
                {
                    ILayer groupLayer = this.Parents;
                    groupLayer.IsRefactoringTransformer = true;
                }
            }
        }
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Style.TransformMultiplies(matrix);
            this.Transform.TransformMultiplies(matrix);
        }
        public virtual void TransformAdd(Vector2 vector)
        {
            this.Style.TransformAdd(vector);
            this.Transform.TransformAdd(vector);
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

            destination.Style = source.Style.Clone();
            destination.Transform = source.Transform.Clone();
            destination.Effect = source.Effect.Clone();
            foreach (IAdjustment adjustment in source.Filter.Adjustments)
            {
                IAdjustment clone = adjustment.Clone();
                destination.Filter.Adjustments.Add(clone);
            }

            foreach (ILayer layer in source.Children)
            {
                ILayer clone = layer.Clone(resourceCreator);
                destination.Children.Add(clone);
            }
        }

    }
}