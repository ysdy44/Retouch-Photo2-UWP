using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using Retouch_Photo2.Layers;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Layers.Models
{
    /// <summary>
    /// <see cref="Layer"/>'s IGeometryLayer .
    /// </summary>
    public abstract class IGeometryLayer : Layer
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
                            {
                                Vector2 startPoint = Vector2.Transform(this.FillBrush.Points.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.FillBrush.Points.LinearGradientEndPoint, canvasToVirtualMatrix);

                                ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.FillBrush.Array)
                                {
                                    StartPoint = startPoint,
                                    EndPoint = endPoint,
                                };

                                drawingSession.FillGeometry(geometry, brush);
                            }
                            break;

                        case BrushType.RadialGradient:
                            {
                                Vector2 center = Vector2.Transform(this.FillBrush.Points.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.FillBrush.Points.RadialGradientPoint, canvasToVirtualMatrix);
                                float radius = Vector2.Distance(center, point);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.FillBrush.Array)
                                {
                                    RadiusX = radius,
                                    RadiusY = radius,
                                    Center = center
                                };

                                drawingSession.FillGeometry(geometry, brush);
                            }
                            break;

                        case BrushType.EllipticalGradient:
                            {
                                Vector2 center = Vector2.Transform(this.FillBrush.Points.EllipticalGradientCenter, canvasToVirtualMatrix);
                                Vector2 xPoint = Vector2.Transform(this.FillBrush.Points.EllipticalGradientXPoint, canvasToVirtualMatrix);
                                Vector2 yPoint = Vector2.Transform(this.FillBrush.Points.EllipticalGradientYPoint, canvasToVirtualMatrix);

                                float radiusX = Vector2.Distance(center, xPoint);
                                float radiusY = Vector2.Distance(center, yPoint);
                                Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                                    * Matrix3x2.CreateRotation(TransformerMath.VectorToRadians(xPoint - center))
                                    * Matrix3x2.CreateTranslation(center);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.FillBrush.Array)
                                {
                                    Transform = transformMatrix,
                                    RadiusX = radiusX,
                                    RadiusY = radiusY,
                                    Center = center
                                };

                                drawingSession.FillGeometry(geometry, brush);
                            }
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
                            {
                                Vector2 startPoint = Vector2.Transform(this.StrokeBrush.Points.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.StrokeBrush.Points.LinearGradientEndPoint, canvasToVirtualMatrix);

                                ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.StrokeBrush.Array)
                                {
                                    StartPoint = startPoint,
                                    EndPoint = endPoint,
                                };

                                drawingSession.DrawGeometry(geometry, brush, strokeWidth);
                            }
                            break;

                        case BrushType.RadialGradient:
                            {
                                Vector2 center = Vector2.Transform(this.StrokeBrush.Points.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.StrokeBrush.Points.RadialGradientPoint, canvasToVirtualMatrix);
                                float radius = Vector2.Distance(center, point);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.StrokeBrush.Array)
                                {
                                    RadiusX = radius,
                                    RadiusY = radius,
                                    Center = center
                                };

                                drawingSession.DrawGeometry(geometry, brush, strokeWidth);
                            }
                            break;

                        case BrushType.EllipticalGradient:
                            {
                                Vector2 center = Vector2.Transform(this.StrokeBrush.Points.EllipticalGradientCenter, canvasToVirtualMatrix);
                                Vector2 xPoint = Vector2.Transform(this.StrokeBrush.Points.EllipticalGradientXPoint, canvasToVirtualMatrix);
                                Vector2 yPoint = Vector2.Transform(this.StrokeBrush.Points.EllipticalGradientYPoint, canvasToVirtualMatrix);

                                float radiusX = Vector2.Distance(center, xPoint);
                                float radiusY = Vector2.Distance(center, yPoint);
                                Matrix3x2 transformMatrix = Matrix3x2.CreateTranslation(-center)
                                    * Matrix3x2.CreateRotation(TransformerMath.VectorToRadians(xPoint - center))
                                    * Matrix3x2.CreateTranslation(center);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.StrokeBrush.Array)
                                {
                                    Transform = transformMatrix,
                                    RadiusX = radiusX,
                                    RadiusY = radiusY,
                                    Center = center
                                };

                                drawingSession.DrawGeometry(geometry, brush, strokeWidth);
                            }
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