using FanKit.Transformers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Retouch_Photo2.Brushs;
using System.Numerics;
using Windows.UI;

namespace Retouch_Photo2.Layers.ILayer
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
        /// /// <param name="resourceCreator"> resourceCreator </param>
        /// <param name="canvasToVirtualMatrix"> canvasToVirtualMatrix </param>
        /// <returns> geometry </returns>
        public abstract CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator, Matrix3x2 canvasToVirtualMatrix);

        
        /// <summary> <see cref = "IGeometryLayer" />'s fill-brush. </summary>
        public Brush FillBrush = new Brush();
        /// <summary> <see cref = "IGeometryLayer" />'s stroke-brush. </summary>
        public Brush StrokeBrush = new Brush();
        /// <summary> <see cref = "IGeometryLayer" />'s stroke-width. </summary>
        public float StrokeWidth = 1;


        //@Override
        public override void SetFillColor(Color fillColor) => this.FillBrush.Color = fillColor;
        public override Color? GetFillColor() => this.FillBrush.Color;

        public override void SetStrokeColor(Color strokeColor) => this.StrokeBrush.Color = strokeColor;
        public override Color? GetStrokeColor() => this.StrokeBrush.Color;

        public override ICanvasImage GetRender(ICanvasResourceCreator resourceCreator, ICanvasImage previousImage, Matrix3x2 canvasToVirtualMatrix)
        {
            CanvasCommandList command = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = command.CreateDrawingSession())
            {
                CanvasGeometry geometry = this.CreateGeometry(resourceCreator, canvasToVirtualMatrix);

                //Fill
                {
                    switch (this.FillBrush.Type)
                    {
                        case BrushType.None:
                            break;

                        case BrushType.Color:
                            ds.FillGeometry(geometry, this.FillBrush.Color);
                            break;

                        case BrushType.LinearGradient:
                            {
                                Vector2 startPoint = Vector2.Transform(this.FillBrush.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.FillBrush.LinearGradientEndPoint, canvasToVirtualMatrix);

                                ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.FillBrush.Array)
                                {
                                    StartPoint = startPoint,
                                    EndPoint = endPoint,
                                };

                                ds.FillGeometry(geometry, brush);
                            }
                            break;

                        case BrushType.RadialGradient:
                            {
                                Vector2 center = Vector2.Transform(this.FillBrush.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.FillBrush.RadialGradientPoint, canvasToVirtualMatrix);
                                float radius = Vector2.Distance(center, point);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.FillBrush.Array)
                                {
                                    RadiusX = radius,
                                    RadiusY = radius,
                                    Center = center
                                };

                                ds.FillGeometry(geometry, brush);
                            }
                            break;

                        case BrushType.EllipticalGradient:
                            {
                                Vector2 center = Vector2.Transform(this.FillBrush.EllipticalGradientCenter, canvasToVirtualMatrix);
                                Vector2 xPoint = Vector2.Transform(this.FillBrush.EllipticalGradientXPoint, canvasToVirtualMatrix);
                                Vector2 yPoint = Vector2.Transform(this.FillBrush.EllipticalGradientYPoint, canvasToVirtualMatrix);

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

                                ds.FillGeometry(geometry, brush);
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
                            ds.DrawGeometry(geometry, this.StrokeBrush.Color, strokeWidth);
                            break;

                        case BrushType.LinearGradient:
                            {
                                Vector2 startPoint = Vector2.Transform(this.StrokeBrush.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.StrokeBrush.LinearGradientEndPoint, canvasToVirtualMatrix);

                                ICanvasBrush brush = new CanvasLinearGradientBrush(resourceCreator, this.StrokeBrush.Array)
                                {
                                    StartPoint = startPoint,
                                    EndPoint = endPoint,
                                };

                                ds.DrawGeometry(geometry, brush, strokeWidth);
                            }
                            break;

                        case BrushType.RadialGradient:
                            {
                                Vector2 center = Vector2.Transform(this.StrokeBrush.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.StrokeBrush.RadialGradientPoint, canvasToVirtualMatrix);
                                float radius = Vector2.Distance(center, point);

                                ICanvasBrush brush = new CanvasRadialGradientBrush(resourceCreator, this.StrokeBrush.Array)
                                {
                                    RadiusX = radius,
                                    RadiusY = radius,
                                    Center = center
                                };

                                ds.DrawGeometry(geometry, brush, strokeWidth);
                            }
                            break;

                        case BrushType.EllipticalGradient:
                            {
                                Vector2 center = Vector2.Transform(this.StrokeBrush.EllipticalGradientCenter, canvasToVirtualMatrix);
                                Vector2 xPoint = Vector2.Transform(this.StrokeBrush.EllipticalGradientXPoint, canvasToVirtualMatrix);
                                Vector2 yPoint = Vector2.Transform(this.StrokeBrush.EllipticalGradientYPoint, canvasToVirtualMatrix);

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

                                ds.DrawGeometry(geometry, brush, strokeWidth);
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
    }
}