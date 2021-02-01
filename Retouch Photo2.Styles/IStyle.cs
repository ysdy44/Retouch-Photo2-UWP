// Core:              ★★★★
// Referenced:   ★★★★★
// Difficult:         ★★
// Only:              ★★★★
// Complete:      ★★★
using FanKit.Transformers;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;

namespace Retouch_Photo2.Styles
{
    /// <summary>
    /// Interface of <see cref="Style"/>.
    /// </summary>
    public interface IStyle : ICacheTransform
    {

        /// <summary> Name </summary>
        string Name { get; set; }

        /// <summary> Gets or sets whether the style follows the transform. </summary>
        bool IsFollowTransform { get; set; }

        /// <summary> Gets or sets whether the stroke is behind the fill. </summary>
        bool IsStrokeBehindFill { get; set; }

        /// <summary> Gets or sets Style's fill. </summary>
        IBrush Fill { get; set; }
        /// <summary> The cache of <see cref="IStyle.Fill"/>. </summary>
        IBrush StartingFill { get; }
        /// <summary> Cache the <see cref="IStyle.Fill"/>. </summary>
        void CacheFill();

        /// <summary> Gets or sets Style's stroke. </summary>
        IBrush Stroke { get; set; }
        /// <summary> The cache of <see cref="IStyle.Stroke"/>. </summary>
        IBrush StartingStroke { get; }
        /// <summary> Cache the <see cref="IStyle.Stroke"/>. </summary>
        void CacheStroke();

        /// <summary> Gets or sets whether the StrokeWidth follows the scale. </summary>
        bool IsStrokeWidthFollowScale { get; set; }

        /// <summary> Gets or sets Style's stroke-width. </summary>
        float StrokeWidth { get; set; }
        /// <summary> The cache of <see cref="IStyle.StrokeWidth"/>. </summary>
        float StartingStrokeWidth { get; }
        /// <summary> Cache the <see cref="IStyle.StrokeWidth"/>. </summary>
        void CacheStrokeWidth();

        /// <summary> Gets or sets Style's stroke-style. </summary>
        CanvasStrokeStyle StrokeStyle { get; set; }
        /// <summary> The cache of <see cref="IStyle.StrokeStyle"/>. </summary>
        CanvasStrokeStyle StartingStrokeStyle { get; }
        /// <summary> Cache the <see cref="IStyle.StrokeStyle"/>. </summary>
        void CacheStrokeStyle();

        /// <summary> Gets or sets Style's transparency. </summary>
        IBrush Transparency { get; set; }
        /// <summary> The cache of <see cref="IStyle.Transparency"/>. </summary>
        IBrush StartingTransparency { get; }
        /// <summary> Cache the <see cref="IStyle.Transparency"/>. </summary>
        void CacheTransparency();


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned style. </returns>
        IStyle Clone();


        /// <summary>
        /// Convert all brush points
        /// from starting transformer
        /// into <see cref="Transformer.One"/>.
        /// </summary>
        /// <param name="startingTransformer"> The starting transformer. </param>
        void OneBrushPoints(Transformer startingTransformer);
        /// <summary>
        /// Convert all brush points
        /// from <see cref="Transformer.One"/>.
        /// into transformer.
        /// </summary>
        /// <param name="transformer"> The Transformer about _oldPoints. </param>   
        void DeliverBrushPoints(Transformer transformer);

    }
}