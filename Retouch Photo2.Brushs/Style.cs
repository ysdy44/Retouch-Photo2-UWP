using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class Style : ICacheTransform
    {
        /// <summary> Gets or sets whether the style follows the transform. </summary>
        public bool IsFollowTransform = true;

        /// <summary> Gets or sets Style's fill. </summary>
        public IBrush Fill = new BrushBase();
        /// <summary> The cache of <see cref="Style.Fill"/>. </summary>
        public IBrush StartingFill { get; private set; }
        /// <summary> Cache the <see cref="Style.Fill"/>. </summary>
        public void CacheFill() => this.StartingFill = this.Fill.Clone();
        
        /// <summary> Gets or sets Style's stroke. </summary>
        public IBrush Stroke = new BrushBase();
        /// <summary> The cache of <see cref="Style.Stroke"/>. </summary>
        public IBrush StartingStroke { get; private set; }
        /// <summary> Cache the <see cref="Style.Stroke"/>. </summary>
        public void CacheStroke() => this.StartingStroke = this.Stroke.Clone();
        
        /// <summary> Gets or sets Style's stroke-width. </summary>
        public float StrokeWidth = 1;
        /// <summary> The cache of <see cref="Style.StrokeWidth"/>. </summary>
        public float StartingStrokeWidth { get; private set; }
        /// <summary> Cache the <see cref="Style.StrokeWidth"/>. </summary>
        void CacheStrokeWidth() => this.StartingStrokeWidth = this.StrokeWidth;
        
        /// <summary> Gets or sets Style's stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle = new CanvasStrokeStyle();
        /// <summary> The cache of <see cref="Style.StrokeStyle"/>. </summary>
        public CanvasStrokeStyle StartingStrokeStyle { get; private set; }
        /// <summary> Cache the <see cref="Style.StrokeStyle"/>. </summary>
        void CacheStrokeStyle() => this.StartingStrokeStyle = this.StrokeStyle.Clone();
               
        /// <summary> Gets or sets Style's transparency. </summary>
        public IBrush Transparency = new BrushBase();
        /// <summary> The cache of <see cref="Style.Transparency"/>. </summary>
        public IBrush StartingTransparency { get; private set; }
        /// <summary> Cache the <see cref="Style.Transparency"/>. </summary>
        public void CacheTransparency() => this.StartingTransparency = this.Transparency.Clone();
        
        //@Interface
        /// <summary>
        ///  Cache the style's transformer.
        /// </summary>
        public void CacheTransform()
        {
            if (this.IsFollowTransform)
            {
                this.Fill.CacheTransform();
                this.Stroke.CacheTransform();
            }
        }
        /// <summary>
        ///  Transforms the style by the given matrix.
        /// </summary>
        /// <param name="matrix"> The sestination matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            if (this.IsFollowTransform)
            {
                this.Fill.TransformMultiplies(matrix);
                this.Stroke.TransformMultiplies(matrix);
            }
        }
        /// <summary>
        ///  Transforms the style by the given vector.
        /// </summary>
        /// <param name="vector"> The sestination vector. </param>
        public void TransformAdd(Vector2 vector)
        {
            if (this.IsFollowTransform)
            {
                this.Fill.TransformAdd(vector);
                this.Stroke.TransformAdd(vector);
            }
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned style. </returns>
        public Style Clone()
        {
            return new Style
            {
                Fill = this.Fill.Clone(),
                Stroke = this.Stroke.Clone(),
                StrokeWidth = this.StrokeWidth,
                StrokeStyle = this.StrokeStyle.Clone()
            };
        }

        
        /// <summary>
        /// Convert all brush points
        /// from starting transformer
        /// into <see cref="Transformer.One"/>.
        /// </summary>
        /// <param name="startingTransformer"> The starting transformer. </param>
        public void OneBrushPoints(Transformer startingTransformer)
        {
            this.Fill.CacheTransform();
            this.Stroke.CacheTransform();

            Matrix3x2 oneMatrix = Transformer.FindHomography(startingTransformer, Transformer.One);
            this.Fill.TransformMultiplies(oneMatrix);
            this.Stroke.TransformMultiplies(oneMatrix);

            this.Stroke.CacheTransform();
            this.Fill.CacheTransform();
        }
        /// <summary>
        /// Convert all brush points
        /// from <see cref="Transformer.One"/>.
        /// into transformer.
        /// </summary>
        /// <param name="transformer"> The Transformer about _oldPoints. </param>   
        public void DeliverBrushPoints(Transformer transformer)
        {
            Matrix3x2 matrix = Transformer.FindHomography(Transformer.One, transformer);

            this.Fill.TransformMultiplies(matrix);
            this.Stroke.TransformMultiplies(matrix);
        }

    }
}