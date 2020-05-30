using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Filters;
using Retouch_Photo2.Styles;
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
        private string name= string.Empty;
        public string Name
        {
            get => this.name;
            set
            {
                this.Control.Text = value;
                this.name = value;
            }
        }
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

        public bool IsRefactoringTransformer { get; set; } = true;
        public virtual Transformer GetActualTransformer(Layerage layerage) => this.Transform.GetActualTransformer();

        public Retouch_Photo2.Styles.Style Style { get; set; } = new Retouch_Photo2.Styles.Style();
        public Transform Transform { get; set; } = new Transform();
        public Effect Effect { get; set; } = new Effect();
        public Filter Filter { get; set; } = new Filter();


        public abstract ILayer Clone(CanvasDevice customDevice);

        public virtual void SaveWith(XElement element) { }
        public virtual void Load(XElement element) { }


        public virtual void CacheTransform()
        {
            this.Style.CacheTransform();
            this.Transform.CacheTransform();
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
            destination.Id = source.Id;

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
        }

    }
}