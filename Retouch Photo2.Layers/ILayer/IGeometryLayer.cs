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
        public override void SetFillColor(Color fillColor)
        {
            this.FillBrush.Color = fillColor;
            
            if (this.FillBrush.Type != BrushType.Color)
            {
                this.FillBrush.Type = BrushType.Color;
            }
        }
        public override Color? GetFillColor() => this.FillBrush.Color;


        public override void SetStrokeColor(Color strokeColor)
        {    
            this.StrokeBrush.Color = strokeColor;

            if (this.StrokeBrush.Type != BrushType.Color)
            {
                this.StrokeBrush.Type = BrushType.Color;
            }
        }
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
                                Vector2 startPoint = Vector2.Transform(this.FillBrush.Points.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.FillBrush.Points.LinearGradientEndPoint, canvasToVirtualMatrix);

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
                                Vector2 center = Vector2.Transform(this.FillBrush.Points.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.FillBrush.Points.RadialGradientPoint, canvasToVirtualMatrix);
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
                                Vector2 startPoint = Vector2.Transform(this.StrokeBrush.Points.LinearGradientStartPoint, canvasToVirtualMatrix);
                                Vector2 endPoint = Vector2.Transform(this.StrokeBrush.Points.LinearGradientEndPoint, canvasToVirtualMatrix);

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
                                Vector2 center = Vector2.Transform(this.StrokeBrush.Points.RadialGradientCenter, canvasToVirtualMatrix);
                                Vector2 point = Vector2.Transform(this.StrokeBrush.Points.RadialGradientPoint, canvasToVirtualMatrix);
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
        public override void DrawBound(ICanvasResourceCreator resourceCreator, CanvasDrawingSession ds, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            CanvasGeometry geometry = this.CreateGeometry(resourceCreator, matrix);
            ds.DrawGeometry(geometry, accentColor);
        }

        
        public override void CacheTransform()
        {
            base.CacheTransform();

            this.FillBrush.OldPoints = this.FillBrush.Points;
            this.StrokeBrush.OldPoints = this.StrokeBrush.Points;
        }
        public override void TransformMultiplies(Matrix3x2 matrix)
        {
            base.TransformMultiplies(matrix);

            switch (this.FillBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.FillBrush.Points.LinearGradientStartPoint = Vector2.Transform(this.FillBrush.OldPoints.LinearGradientStartPoint, matrix); ;
                        this.FillBrush.Points.LinearGradientEndPoint = Vector2.Transform(this.FillBrush.OldPoints.LinearGradientEndPoint, matrix); ;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.FillBrush.Points.RadialGradientCenter = Vector2.Transform(this.FillBrush.OldPoints.RadialGradientCenter, matrix); ;
                        this.FillBrush.Points.RadialGradientPoint = Vector2.Transform(this.FillBrush.OldPoints.RadialGradientPoint, matrix); ;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.FillBrush.Points.EllipticalGradientCenter = Vector2.Transform(this.FillBrush.OldPoints.EllipticalGradientCenter, matrix); ;
                        this.FillBrush.Points.EllipticalGradientXPoint = Vector2.Transform(this.FillBrush.OldPoints.EllipticalGradientXPoint, matrix); ;
                        this.FillBrush.Points.EllipticalGradientYPoint = Vector2.Transform(this.FillBrush.OldPoints.EllipticalGradientYPoint, matrix); ;
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }

            switch (this.StrokeBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.StrokeBrush.Points.LinearGradientStartPoint = Vector2.Transform(this.StrokeBrush.OldPoints.LinearGradientStartPoint, matrix); ;
                        this.StrokeBrush.Points.LinearGradientEndPoint = Vector2.Transform(this.StrokeBrush.OldPoints.LinearGradientEndPoint, matrix); ;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.StrokeBrush.Points.RadialGradientCenter = Vector2.Transform(this.StrokeBrush.OldPoints.RadialGradientCenter, matrix); ;
                        this.StrokeBrush.Points.RadialGradientPoint = Vector2.Transform(this.StrokeBrush.OldPoints.RadialGradientPoint, matrix); ;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.StrokeBrush.Points.EllipticalGradientCenter = Vector2.Transform(this.StrokeBrush.OldPoints.EllipticalGradientCenter, matrix); ;
                        this.StrokeBrush.Points.EllipticalGradientXPoint = Vector2.Transform(this.StrokeBrush.OldPoints.EllipticalGradientXPoint, matrix); ;
                        this.StrokeBrush.Points.EllipticalGradientYPoint = Vector2.Transform(this.StrokeBrush.OldPoints.EllipticalGradientYPoint, matrix); ;
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }
        }
        public override void TransformAdd(Vector2 vector)
        {
            base.TransformAdd(vector);

            switch (this.FillBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.FillBrush.Points.LinearGradientStartPoint = Vector2.Add(this.FillBrush.OldPoints.LinearGradientStartPoint, vector); ;
                        this.FillBrush.Points.LinearGradientEndPoint = Vector2.Add(this.FillBrush.OldPoints.LinearGradientEndPoint, vector); ;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.FillBrush.Points.RadialGradientCenter = Vector2.Add(this.FillBrush.OldPoints.RadialGradientCenter, vector); ;
                        this.FillBrush.Points.RadialGradientPoint = Vector2.Add(this.FillBrush.OldPoints.RadialGradientPoint, vector); ;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.FillBrush.Points.EllipticalGradientCenter = Vector2.Add(this.FillBrush.OldPoints.EllipticalGradientCenter, vector); ;
                        this.FillBrush.Points.EllipticalGradientXPoint = Vector2.Add(this.FillBrush.OldPoints.EllipticalGradientXPoint, vector); ;
                        this.FillBrush.Points.EllipticalGradientYPoint = Vector2.Add(this.FillBrush.OldPoints.EllipticalGradientYPoint, vector); ;
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }

            switch (this.StrokeBrush.Type)
            {
                case BrushType.None:
                    break;
                case BrushType.Color:
                    break;
                case BrushType.LinearGradient:
                    {
                        this.StrokeBrush.Points.LinearGradientStartPoint = Vector2.Add(this.StrokeBrush.OldPoints.LinearGradientStartPoint, vector); ;
                        this.StrokeBrush.Points.LinearGradientEndPoint = Vector2.Add(this.StrokeBrush.OldPoints.LinearGradientEndPoint, vector); ;
                    }
                    break;
                case BrushType.RadialGradient:
                    {
                        this.StrokeBrush.Points.RadialGradientCenter = Vector2.Add(this.StrokeBrush.OldPoints.RadialGradientCenter, vector); ;
                        this.StrokeBrush.Points.RadialGradientPoint = Vector2.Add(this.StrokeBrush.OldPoints.RadialGradientPoint, vector); ;
                    }
                    break;
                case BrushType.EllipticalGradient:
                    {
                        this.StrokeBrush.Points.EllipticalGradientCenter = Vector2.Add(this.StrokeBrush.OldPoints.EllipticalGradientCenter, vector); ;
                        this.StrokeBrush.Points.EllipticalGradientXPoint = Vector2.Add(this.StrokeBrush.OldPoints.EllipticalGradientXPoint, vector); ;
                        this.StrokeBrush.Points.EllipticalGradientYPoint = Vector2.Add(this.StrokeBrush.OldPoints.EllipticalGradientYPoint, vector); ;
                    }
                    break;
                case BrushType.Image:
                    break;
                default:
                    break;
            }
        }

    }
}