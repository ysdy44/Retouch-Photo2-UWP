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
    public partial class StyleManager : ICacheTransform
    {
        /// <summary> Gets or sets Style's fill-brush. </summary>
        public Brush FillBrush = new Brush();
        /// <summary> Gets or sets Style's stroke-brush. </summary>
        public Brush StrokeBrush = new Brush();
        /// <summary> Gets or sets Style's stroke-width. </summary>
        public float StrokeWidth = 1;


        //@Interface
        /// <summary>
        ///  Cache the style's transformer.
        /// </summary>
        public void CacheTransform()
        {
            this.FillBrush.CacheTransform();
            this.StrokeBrush.CacheTransform();
        }
        /// <summary>
        ///  Transforms the style by the given matrix.
        /// </summary>
        /// <param name="matrix"> The sestination matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.FillBrush.TransformMultiplies(matrix);
            this.StrokeBrush.TransformMultiplies(matrix);
        }
        /// <summary>
        ///  Transforms the style by the given vector.
        /// </summary>
        /// <param name="vector"> The sestination vector. </param>
        public void TransformAdd(Vector2 vector)
        {
            this.FillBrush.TransformAdd(vector);
            this.StrokeBrush.TransformAdd(vector);
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
            switch (this.FillBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    drawingSession.FillGeometry(geometry, this.FillBrush.Color);
                    break;
                case BrushType.LinearGradient:
                    ICanvasBrush linearGradient = this.FillBrush.GetLinearGradient(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.FillGeometry(geometry, linearGradient);
                    break;
                case BrushType.RadialGradient:
                    ICanvasBrush radialGradientBrush = this.FillBrush.GetRadialGradientBrush(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.FillGeometry(geometry, radialGradientBrush);
                    break;
                case BrushType.EllipticalGradient:
                    ICanvasBrush ellipticalGradientBrush = this.FillBrush.GetEllipticalGradientBrush(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.FillGeometry(geometry, ellipticalGradientBrush);
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }
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
            float strokeWidth = this.StrokeWidth * (canvasToVirtualMatrix.M11 + canvasToVirtualMatrix.M22) / 2;

            switch (this.StrokeBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    drawingSession.DrawGeometry(geometry, this.StrokeBrush.Color, strokeWidth);
                    break;
                case BrushType.LinearGradient:
                    ICanvasBrush linearGradient = this.StrokeBrush.GetLinearGradient(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.DrawGeometry(geometry, linearGradient, strokeWidth);
                    break;
                case BrushType.RadialGradient:
                    ICanvasBrush radialGradientBrush = this.StrokeBrush.GetRadialGradientBrush(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.DrawGeometry(geometry, radialGradientBrush, strokeWidth);
                    break;
                case BrushType.EllipticalGradient:
                    ICanvasBrush ellipticalGradientBrush = this.StrokeBrush.GetEllipticalGradientBrush(resourceCreator, canvasToVirtualMatrix);
                    drawingSession.DrawGeometry(geometry, ellipticalGradientBrush, strokeWidth);
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }
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
            };
        }
        /// <summary>
        /// Copy a StyleManager with self.
        /// </summary>
        public void CopyWith(StyleManager styleManager)
        {
            this.FillBrush = styleManager.FillBrush.Clone();
            this.StrokeBrush = styleManager.StrokeBrush.Clone();
            this.StrokeWidth = styleManager.StrokeWidth;
        }
    }
}