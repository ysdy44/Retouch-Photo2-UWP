// Core:              ★★★★
// Referenced:   ★★★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Strokes;
using System.Collections.Generic;
using System.Numerics;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class Style : IStyle
    {

        /// <summary> Name </summary>
        public string Name { get; set; } 

        /// <summary>
        /// The localized strings resources.
        /// </summary>
        public IDictionary<string, string> Strings { get; set; }

        /// <summary> Gets or sets whether the style follows the transform. </summary>
        public bool IsFollowTransform { get; set; } = true;

        /// <summary> Gets or sets whether the stroke is behind the fill.. </summary>
        public bool IsStrokeBehindFill { get; set; } = false;

        /// <summary> Gets or sets Style's fill. </summary>
        public IBrush Fill { get; set; } = new BrushBase();
        /// <summary> The cache of <see cref="IStyle.Fill"/>. </summary>
        public IBrush StartingFill { get; private set; }
        /// <summary> Cache the <see cref="IStyle.Fill"/>. </summary>
        public void CacheFill() => this.StartingFill = this.Fill.Clone();
        
        /// <summary> Gets or sets Style's stroke. </summary>
        public IBrush Stroke { get; set; } = new BrushBase();
        /// <summary> The cache of <see cref="IStyle.Stroke"/>. </summary>
        public IBrush StartingStroke { get; private set; }
        /// <summary> Cache the <see cref="IStyle.Stroke"/>. </summary>
        public void CacheStroke() => this.StartingStroke = this.Stroke.Clone();

        /// <summary> Gets or sets whether the StrokeWidth follows the scale. </summary>
        public bool IsStrokeWidthFollowScale { get; set; } = false;

        /// <summary> Gets or sets Style's stroke-width. </summary>
        public float StrokeWidth { get; set; } = 1;
        /// <summary> The cache of <see cref="IStyle.StrokeWidth"/>. </summary>
        public float StartingStrokeWidth { get; private set; }
        /// <summary> Cache the <see cref="IStyle.StrokeWidth"/>. </summary>
        public void CacheStrokeWidth() => this.StartingStrokeWidth = this.StrokeWidth;
        
        /// <summary> Gets or sets Style's stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle { get; set; } = new CanvasStrokeStyle();
        /// <summary> The cache of <see cref="IStyle.StrokeStyle"/>. </summary>
        public CanvasStrokeStyle StartingStrokeStyle { get; private set; }
        /// <summary> The cache of <see cref="CanvasStrokeStyle.DashOffset"/>. </summary>
        public float StartingOffset { get; set; }
        /// <summary> Cache the <see cref="IStyle.StrokeStyle"/>. </summary>
        public void CacheStrokeStyle() => this.StartingStrokeStyle = this.StrokeStyle.Clone();
               
        /// <summary> Gets or sets Style's transparency. </summary>
        public IBrush Transparency { get; set; } = new BrushBase();
        /// <summary> The cache of <see cref="IStyle.Transparency"/>. </summary>
        public IBrush StartingTransparency { get; private set; }
        /// <summary> Cache the <see cref="IStyle.Transparency"/>. </summary>
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
                this.Transparency.CacheTransform();
            }

            if (this.IsStrokeWidthFollowScale)
            {
                this.CacheStrokeWidth();
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
                this.Transparency.TransformMultiplies(matrix);
            }

            if (this.IsStrokeWidthFollowScale)
            {
                float scale = matrix.M11 + matrix.M22;
                this.StrokeWidth = this.StartingStrokeWidth * scale / 2.0f;
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
                this.Transparency.TransformAdd(vector);
            }

            //if (this.IsStrokeWidthFollowScale)
            //{
            //}
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned style. </returns>
        public IStyle Clone()
        {
            return new Style
            {
                IsFollowTransform = this.IsFollowTransform,

                IsStrokeBehindFill = this.IsStrokeBehindFill,

                Fill = this.Fill.Clone(),
                Stroke = this.Stroke.Clone(),

                IsStrokeWidthFollowScale = this.IsStrokeWidthFollowScale,

                StrokeWidth = this.StrokeWidth,
                StrokeStyle = this.StrokeStyle.Clone(),
                Transparency = this.Transparency.Clone(),
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
            this.Transparency.CacheTransform();

            Matrix3x2 oneMatrix = Transformer.FindHomography(startingTransformer, Transformer.One);
            this.Fill.TransformMultiplies(oneMatrix);
            this.Stroke.TransformMultiplies(oneMatrix);
            this.Transparency.TransformMultiplies(oneMatrix);

            this.Stroke.CacheTransform();
            this.Fill.CacheTransform();
            this.Transparency.CacheTransform();
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
            this.Transparency.TransformMultiplies(matrix);
        }

    }
}