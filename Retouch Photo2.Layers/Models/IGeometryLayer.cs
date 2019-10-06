using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="LayerBase"/>'s IGeometryLayer .
    /// </summary>
    public abstract class IGeometryLayer : LayerBase
    {
        //@Abstract
        /// <summary>
        /// Create a specific geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="canvasToVirtualMatrix"> The canvas-to-virtual matrix. </param>
        /// <returns> geometry </returns>
        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);


        /// <summary> <see cref = "IGeometryLayer" />'s fill-brush. </summary>
        public Brush FillBrush = new Brush();
        /// <summary> <see cref = "IGeometryLayer" />'s stroke-brush. </summary>
        public Brush StrokeBrush = new Brush();
        /// <summary> <see cref = "IGeometryLayer" />'s stroke-width. </summary>
        public float StrokeWidth = 1;
        

        //@Override
        public override Color? FillColor
        {
            get => this.FillBrush.Color;
            set
            {
                if (this.FillBrush.Type != BrushType.Color)
                {
                    this.FillBrush.Type = BrushType.Color;
                }

                if (value is Color color)
                {
                    this.FillBrush.Color = color;
                }
            }
        }
        public override Color? StrokeColor
        {
            get => this.StrokeBrush.Color;
            set
            {
                if (this.StrokeBrush.Type != BrushType.Color)
                {
                    this.StrokeBrush.Type = BrushType.Color;
                }

                if (value is Color color)
                {
                    this.StrokeBrush.Color = color;
                }
            }
        }


        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.CreateGeometry(resourceCreator, canvasToVirtualMatrix);

                //Fill
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

                //Stroke
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
            }
            return command;
        }
        public override void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession drawingSession, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator, matrix);
            drawingSession.DrawGeometry(geometry, accentColor);
        }


        public override void CacheTransform()
        {
            base.CacheTransform();

            this.FillBrush.CacheTransform();
            this.StrokeBrush.CacheTransform();
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);

            this.FillBrush.TransformMultiplies(matrix);
            this.StrokeBrush.TransformMultiplies(matrix);
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);

            this.FillBrush.TransformAdd(vector);
            this.StrokeBrush.TransformAdd(vector);
        }       
    }
}