using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs.Models;
using Retouch_Photo2.Elements;
using System.Numerics;

namespace Retouch_Photo2.Brushs
{
    /// <summary>
    /// Define the object used to draw geometry.
    /// </summary>
    public partial class StyleManager : ICacheTransform
    {
        /// <summary> Gets or sets whether the style follows the transform. </summary>
        public bool IsFollowTransform = true;

        /// <summary> Gets or sets Style's fill-brush. </summary>
        public IBrush FillBrush = new NoneBrush();
        /// <summary> Gets or sets Style's stroke-brush. </summary>
        public IBrush StrokeBrush = new NoneBrush();
        /// <summary> Gets or sets Style's stroke-width. </summary>
        public float StrokeWidth = 1;
        /// <summary> Gets or sets Style's stroke-style. </summary>
        public CanvasStrokeStyle StrokeStyle = new CanvasStrokeStyle();

        //@Interface
        /// <summary>
        ///  Cache the style's transformer.
        /// </summary>
        public void CacheTransform()
        {
            if (this.IsFollowTransform)
            {
                this.FillBrush.CacheTransform();
                this.StrokeBrush.CacheTransform();
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
                this.FillBrush.TransformMultiplies(matrix);
                this.StrokeBrush.TransformMultiplies(matrix);
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
                this.FillBrush.TransformAdd(vector);
                this.StrokeBrush.TransformAdd(vector);
            }
        }


        /// <summary>
        /// Fill a geometry with style.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="geometry"> The source geometry. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-virtual-matrix. </param>
        public void FillGeometry(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry, Matrix3x2 canvasToVirtualMatrix)
        {
            ICanvasBrush canvasBrush = this.FillBrush.GetICanvasBrush(resourceCreator, canvasToVirtualMatrix);
            if (canvasBrush == null) return;

            drawingSession.FillGeometry(geometry, canvasBrush);
        }
        /// <summary>
        /// Draw a geometry with style.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="geometry"> The source geometry. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-virtual-matrix. </param>
        public void DrawGeometry(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, CanvasGeometry geometry, Matrix3x2 canvasToVirtualMatrix)
        {
            ICanvasBrush canvasBrush = this.StrokeBrush.GetICanvasBrush(resourceCreator, canvasToVirtualMatrix);
            if (canvasBrush == null) return;

            float strokeWidth = this.StrokeWidth * (canvasToVirtualMatrix.M11 + canvasToVirtualMatrix.M22) / 2;
            drawingSession.DrawGeometry(geometry, canvasBrush, strokeWidth, this.StrokeStyle);
        }


        /// <summary>
        /// Get StyleManager own copy.
        /// </summary>
        /// <returns> The cloned StyleManager. </returns>
        public StyleManager Clone()
        {
            return new StyleManager
            {
                FillBrush = this.FillBrush.Clone(),
                StrokeBrush = this.StrokeBrush.Clone(),
                StrokeWidth = this.StrokeWidth,
                StrokeStyle = new CanvasStrokeStyle
                {

                }
            };
        }

    }
}