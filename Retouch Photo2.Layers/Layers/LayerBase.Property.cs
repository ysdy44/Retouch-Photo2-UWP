using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Retouch_Photo2.Adjustments;
using Retouch_Photo2.Blends;
using Retouch_Photo2.Effects;
using Retouch_Photo2.Filters;
using System.Numerics;
using System.Xml.Linq;
using Windows.UI.Xaml;

namespace Retouch_Photo2.Layers
{
    /// <summary>
    /// Represents a layer that can have render properties. Provides a rendering method.
    /// </summary>
    public abstract partial class LayerBase
    {

        /// <summary> Gets or sets <see cref = "LayerBase" />'s control. </summary>
        public LayerControl Control { get; protected set; }


        //@Abstract
        /// <summary> Gets the type. </summary>
        public abstract LayerType Type { get; }
        /// <summary> Gets or sets the name. </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.Control.Name2 = value;
                this.name = value;
            }
        }
        private string name= string.Empty;
        /// <summary> Gets or sets the blend mode. </summary>
        public BlendEffectMode? BlendMode { get; set; } = null;

        /// <summary> Gets or sets the opacity. </summary>
        public float Opacity { get; set; } = 1.0f;
        /// <summary> The cache of <see cref="LayerBase.Opacity"/>. </summary>
        public float StartingOpacity { get; private set; } = 1.0f;
        /// <summary> Cache <see cref="LayerBase.Opacity"/>. </summary>
        public void CacheOpacity() => this.StartingOpacity = this.Opacity;

        /// <summary> Gets or sets the visibility. </summary>
        public Visibility Visibility
        {
            get => this.visibility;
            set
            {
                this.Control.Visibility2 = value;
                this.visibility = value;
            }
        }
        private Visibility visibility;
        /// <summary> Gets or sets the tag type. </summary>
        public TagType TagType
        {
            get => this.tagType;
            set
            {
                this.Control.TagType=value;
                this.tagType = value;
            }
        }
        private TagType tagType;

        /// <summary> Gets or sets the expand. </summary>
        public bool IsExpand
        {
            get => this.isExpand;
            set
            {
                this.Control.IsExpand = value;
                this.isExpand = value;
            }
        }
        private bool isExpand;
        /// <summary> Gets or sets the selected. </summary>
        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.Control.IsSelected = value;
                this.isSelected = value;
            }
        }
        private bool isSelected;


        /// <summary> Gets or sets ILayer is need to refactoring transformer. </summary>
        public bool IsRefactoringTransformer { get; set; } = true;

        /// <summary>
        /// Gets the actually transformer.
        /// </summary>
        /// <param name="layerage"> The layerage. </param>
        public virtual Transformer GetActualTransformer(Layerage layerage) => this.Transform.GetActualTransformer();


        /// <summary> Gets or sets the style. </summary>
        public Retouch_Photo2.Styles.Style Style { get; set; } = new Retouch_Photo2.Styles.Style();
        /// <summary> Gets or sets the transform. </summary>
        public Transform Transform { get; set; } = new Transform();
        /// <summary> Gets or sets the effect. </summary>
        public Effect Effect { get; set; } = new Effect();
        /// <summary> Gets or sets the filter. </summary>
        public Filter Filter { get; set; } = new Filter();


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <param name="customDevice"> The custom-device. </param>
        /// <returns> The cloned <see cref="ILayer"/>. </returns>
        public abstract ILayer Clone(CanvasDevice customDevice);

        /// <summary>
        /// Saves the entire <see cref="ILayer"/> to a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        public virtual void SaveWith(XElement element) { }
        /// <summary>
        /// Load the entire <see cref="ILayer"/> form a XElement.
        /// </summary>
        /// <param name="element"> The destination XElement. </param>
        public virtual void Load(XElement element) { }


        /// <summary>
        /// Cache the class's transformer. Ex: _oldTransformer = Transformer.
        /// </summary>
        public virtual void CacheTransform()
        {
            this.Style.CacheTransform();
            this.Transform.CacheTransform();
        }
        /// <summary>
        /// Transforms the class by the given vector. Ex: Transformer.Add()
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public virtual void TransformAdd(Vector2 vector)
        {
            this.Style.TransformAdd(vector);
            this.Transform.TransformAdd(vector);
        }
        /// <summary>
        /// Transforms the class by the given matrix. Ex: Transformer.Multiplies()
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public virtual void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Style.TransformMultiplies(matrix);
            this.Transform.TransformMultiplies(matrix);
        }


        //@Static
        /// <summary>
        /// Copy with self.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="source"> The source <see cref="ILayer"/>. </param>
        /// <param name="destination"> The destination <see cref="ILayer"/>. </param>
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